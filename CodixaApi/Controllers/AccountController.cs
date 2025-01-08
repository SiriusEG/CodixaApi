using Codixa.Core.Models.UserModels;
using Codixa.EF.Dtos.AccountDtos;
using Codxia.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AccountController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        [HttpPost("register")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
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

                return Ok(new
                {
                    message = "User registered successfully"

                });
            }

            return BadRequest(result.Errors);
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }

            var user = await _unitOfWork.UsersManger.FindByNameAsync(userDto.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            var isPasswordValid = await _unitOfWork.UsersManger.CheckPasswordAsync(user, userDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized();
            }

            var roles = await _unitOfWork.UsersManger.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return Ok(new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(1),
            });
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
