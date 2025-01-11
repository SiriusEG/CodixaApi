using Azure.Core;
using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _AuthenticationService;
     

        public AccountController(IAuthenticationService authenticationService)
        {
            _AuthenticationService = authenticationService;
        }


        [HttpPost("register")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            // Create the user
            var result = await _AuthenticationService.RegisterInstructorAsync(model);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    Message = "User registered successfully."
                });
            }

            // Return errors if registration failed
            return BadRequest(new
            {
                Message = "User registration failed.",
                Errors = result.Errors
            });
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }

            try
            {
                var token = await _AuthenticationService.LoginAsync(userDto);
                return Ok(new { Token = token });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidPasswordException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }


     


    }
}
