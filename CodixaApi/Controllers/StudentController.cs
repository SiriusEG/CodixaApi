using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
           _studentService = studentService;
        }


        //send Request enroll course
        [HttpPost("StudentRequestToEnrollCourse/{CourseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentRequestToEnrollCourse([FromRoute] int CourseId)
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
                var result = await _studentService.RequestToEnrollCourse(CourseId, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        //get my courses
        [HttpGet("GetStudentCourses")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentCoursesByToken()
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
                var result = await _studentService.GetStudentCoursesByToken(token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }

        //edit profile
        //changepassword







    }
}
