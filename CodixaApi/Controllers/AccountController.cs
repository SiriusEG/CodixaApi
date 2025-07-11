using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.AccountDtos.Response;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        [HttpPost("RegisterNewStudent")]
        public async Task<IActionResult> RegisterStudent([FromForm] RegisterStudentDto model)
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
            var result = await _AuthenticationService.RegisterStudentAsync(model);

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

        [HttpPost("RegisterNewInstructor")]
        public async Task<IActionResult> RegisterInstructor([FromForm] RegisterInstructorDto model)
        {
          
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
               
                var result = await _AuthenticationService.RegisterInstructorAsync(model);

                if (result.Succeeded)
                {
                  
                    return Ok(new
                    {
                        Message = "Your Request successfully Sent."
                    });
                }

               
                return BadRequest(new
                {
                    Message = "Your Request failed to Sent, " + result.Errors.First().Description,
                   
                });
            }
            catch (FileUplodingException ex)
            {
                
                return BadRequest(new
                {
                    Message = "File upload failed." + ex,
                   
                });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred.",
                  
                });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                LoginTokenDto token = await _AuthenticationService.LoginAsync(userDto);
                return Ok(token);
            }
            catch (UserNotFoundException)
            {
                return NotFound(new { Message = "User Name Is Not Found" });
            }
            catch (InvalidPasswordException)
            {
                return Unauthorized(new { Message = "Entered Password Is Not Correct" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] LoginTokenDto loginToken)
        {
            if (!ModelState.IsValid)
            {
                // Return a 400 Bad Request if the model is invalid
                return BadRequest("Invalid request data.");
            }

            try
            {
                // Call the service method to refresh the token
                var result = await _AuthenticationService.RefreshToken(loginToken);

                // Return the new tokens as a 200 OK response
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Handle unauthorized access (e.g., invalid or expired refresh token)
                return Unauthorized(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update errors (e.g., failed to update or add refresh token)
                return StatusCode(500, "An error occurred while updating the database.");
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [Authorize(Roles = "Student")]
        [HttpGet("GetStudentData")]
        public async Task<IActionResult> GetStudentData()
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {

                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                var StudentData = await _AuthenticationService.GetStudentData(token);

                if (StudentData == null)
                {
                    return NotFound(new { Message = "Student not found." });
                }

                return Ok(new
                {
                    Data = StudentData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message
                });
            }
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("GetInstructorData")]
        public async Task<IActionResult> GetInstructorData()
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {

                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                var InstructorData = await _AuthenticationService.GetInstructorData(token);

                if (InstructorData == null)
                {
                    return NotFound(new { Message = "Instructor not found." });
                }

                return Ok(new
                {
                    Data = InstructorData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPut("ChangeInstructorData")]
        public async Task<IActionResult> ChangeInstructorData([FromForm] ChangeInstructorDataDto dto)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {

                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                var updatedInstructor = await _AuthenticationService.ChangeInstructorData(token, dto);

                if (updatedInstructor == null)
                {
                    return NotFound(new { Message = "Instructor not found." });
                }

                return Ok(new
                {
                    Message = "Instructor updated successfully.",
                    Data = updatedInstructor
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message
                });
            }
        }
        [Authorize]
        [HttpPut("ChangeStudentData")]
        public async Task<IActionResult> ChangeStudentData([FromForm] ChangeStudentDataDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                var updatedStudent = await _AuthenticationService.ChangeStudentData(token,  dto);

                if (updatedStudent == null)
                {
                    return NotFound(new { Message = "Student not found." });
                }

                return Ok(new
                {
                    Message = "Student updated successfully.",
                    Data = updatedStudent
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message
                });
            }
        }


        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserWithConfirmDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                var Password = await _AuthenticationService.ChangeUserPassword(token, dto);

                if (!Password)
                {
                    return NotFound(new { Message = "Password not Correct." });
                }

                return Ok(new
                {
                    Message = "Password updated successfully.",
             
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while updating Password.",
                    Error = ex.Message
                });
            }
        }
    }
}
