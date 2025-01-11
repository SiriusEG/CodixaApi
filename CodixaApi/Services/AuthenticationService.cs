using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Codxia.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodixaApi.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthenticationService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration; 
        }


        public async Task<IdentityResult> RegisterInstructorAsync(RegisterUserDto model)
        {
            var user = new AppUser { UserName = model.UserName, Email = model.Email };
            // Create the user
            var result = await _unitOfWork.UsersManger.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign role to the user
                await _unitOfWork.UsersManger.AddToRoleAsync(user, model.Role);


                // Save the changes to the database
                await _unitOfWork.Complete();

                return result;
            }
            else
            {
                return result;
            }
                
        }

        public async Task<string> LoginAsync(LoginUserDto model)
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

            var roles = await _unitOfWork.UsersManger.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);
            return token;
        }




        private string GenerateJwtToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

 
    }
}
