using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.AccountDtos.Response;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CodixaApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public async Task<IdentityResult> RegisterStudentAsync(RegisterStudentDto model)
        {
            FileEntity file = new FileEntity();
            if (model.Photo != null)
            {

                file = await _unitOfWork.Files.UploadFileAsync(model.Photo, Path.Combine("uploads", "UsersPics"));
                await _unitOfWork.Files.AddAsync(file);
                await _unitOfWork.Complete();
            }
      
            

            var user = new AppUser {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth ,
                PhotoId = file.FileId
            
            };
            // Create the user
            var result = await _unitOfWork.UsersManger.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign role to the user
                await _unitOfWork.UsersManger.AddToRoleAsync(user, "Student");

                await _unitOfWork.Students.AddAsync(new Student
                {
                    StudentFullName = model.FullName,
                    UserId = user.Id
                });
                // Save the changes to the database
                await _unitOfWork.Complete();

                return result;
            }
            else
            {
                return result;
            }

        }


        public async Task<IdentityResult> RegisterInstructorAsync(RegisterInstructorDto model)
        {
            // Start a transaction
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Step 1: Upload the file
                FileEntity file = null;
                FileEntity Photo = new FileEntity();
                if (model.Photo != null)
                {

                    Photo = await _unitOfWork.Files.UploadFileAsync(model.Photo, Path.Combine("uploads", "UsersPics"));
                    await _unitOfWork.Files.AddAsync(Photo);
                    await _unitOfWork.Complete();
                }

                try
                {
                    file = await _unitOfWork.Files.UploadFileAsync(model.Cv, Path.Combine("uploads", "InstructorsCVs"));
                }
                catch (Exception ex)
                {
                    // Handle file upload error
                    throw new FileUplodingException("File uploading failed: " + ex.Message);
                }

                if (file == null)
                {
                    throw new FileUplodingException("File uploading failed: No file was uploaded.");
                }

                // Step 2: Create the user
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    DateOfBirth = model.DateOfBirth,
                    PhotoId = Photo.FileId
                };

                var result = await _unitOfWork.UsersManger.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    // Roll back the transaction if user creation fails
                    await transaction.RollbackAsync();
                    await _unitOfWork.Files.DeleteExistsFileAsync(file.FilePath);
                    return result;
                }

                // Step 3: Save the file entity
                try
                {
                    await _unitOfWork.Files.AddAsync(file);
              
                }
                catch (Exception ex)
                {
                    // Roll back the transaction if file save fails
                    await transaction.RollbackAsync();
                    await _unitOfWork.Files.DeleteExistsFileAsync(file.FilePath);
                    throw new Exception("Failed to save file: " + ex.Message);
                }

                // Step 4: Save the instructor request
                try
                {
                    var instructorRequest = new InstructorJoinRequest
                    {
                        FullName = model.FullName,
                        UserId = user.Id,
                        Specialty = model.Specialty,
                        CvFileId = file.FileId,
                        SubmittedAt = DateTime.Now
                    };

                    await _unitOfWork.InstructorJoinRequests.AddAsync(instructorRequest);
                }
                catch (Exception ex)
                {
                    // Roll back the transaction if instructor save fails
                    await transaction.RollbackAsync();
                    await _unitOfWork.Files.DeleteExistsFileAsync(file.FilePath);
                    throw new Exception("Failed to save instructor request: " + ex.Message);
                }

                // Step 5: Commit the transaction if everything succeeds
                await _unitOfWork.Complete();
                await transaction.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                // Roll back the transaction if any other error occurs
                await transaction.RollbackAsync();
                throw new Exception("Failed to register instructor: " + ex.Message);
            }
        }


   

        public async Task<LoginTokenDto> LoginAsync(LoginUserDto model)
        {
            var user = await _unitOfWork.UsersManger.FindByNameAsync(model.UserName);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var isPasswordValid = await _unitOfWork.UsersManger.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Invalid password.");
            }
           
            var expiresRefrshTokeninDayes = int.Parse(_configuration["JWT:ExpiresRefreshToken"]);
            RefreshToken refreshToken = new RefreshToken
            {
                UserId = user.Id,
                refreshToken = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(expiresRefrshTokeninDayes),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.Complete();

            var token = await GenerateJwtToken(user.Id);



            return new LoginTokenDto { Token = token,RefreshToken = refreshToken.refreshToken };
        }


        public async Task<LoginTokenDto> RefreshToken(LoginTokenDto model)
        {
            var RecivedUserId = await GetUserIdFromToken(model.Token);
            var storedToken = await _unitOfWork.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.refreshToken == model.RefreshToken && rt.UserId == RecivedUserId);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            storedToken.IsRevoked = true;
            await _unitOfWork.RefreshTokens.UpdateAsync(storedToken);

            var newAccessToken = await GenerateJwtToken(RecivedUserId);
            var expiresRefrshTokeninDayes = int.Parse(_configuration["JWT:ExpiresRefreshToken"]);
            var newRefreshToken = new RefreshToken
            {
                UserId = RecivedUserId,
                refreshToken = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(expiresRefrshTokeninDayes),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
            await _unitOfWork.Complete();

            return new LoginTokenDto {RefreshToken = newRefreshToken.refreshToken, Token = newAccessToken};
        }

        private async Task<string> GenerateJwtToken(string userId)
        {

            var User = await _unitOfWork.UsersManger.FirstOrDefaultAsync(x=>x.Id == userId, includes=>includes.Include(c=>c.Photo));


            var roles = await _unitOfWork.UsersManger.GetRolesAsync(User);



            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, User.UserName),
                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("ProfilePicturePath", User.Photo == null ? "null" : User.Photo.FilePath.ToString()) 

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiresInMinutes = int.Parse(_configuration["JWT:Expires"]);


            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }



        public async Task<string> GetUserIdFromToken(string token)
        {
            try
            {
                // Remove "Bearer " prefix if present
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }

                // Create a JWT token handler
                var tokenHandler = new JwtSecurityTokenHandler();

                // Read the token without validating it (use only for trusted tokens)
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Extract the user ID from the claims
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub" || claim.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }

                throw new Exception("User ID claim not found in the token.");
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., invalid token format)
                throw new Exception("Failed to extract user ID from token.", ex);
            }
        }
    }
}
