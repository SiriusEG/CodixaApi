using Codixa.Core.Dtos.InstructorDtos.Request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet("GetStudentsRequestsByCourse/{CourseId}/{PageNumber}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> StudentRequestToEnrollCourse([FromRoute] int CourseId, [FromRoute]int pageNumber)
        {
            try
            {
                // Extract the token from the Authorization header
                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                // Pass the token and DTO to the service class
                var result = await _instructorService.GetStudentRequestToEnrollCourse(token, CourseId, pageNumber,10);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        [HttpPost("ChangeStudentEnrollStatus")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ChangeStudentEnrollStatus(ChangeStudentRequestDto changeStudentRequestDto)
        {
            try
            {
                // Extract the token from the Authorization header
                string token = null;
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
                {
                    token = authHeader.ToString().Replace("Bearer ", "").Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(new { Message = "Authorization token is missing." });
                }

                // Pass the token and DTO to the service class
                var result = await _instructorService.ChangeStudentRequestStatus(token, changeStudentRequestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }


    }
}
