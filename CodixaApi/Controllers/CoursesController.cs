using Azure.Core;
using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService) {
        
            _courseService = courseService; 
        
        }
        //add course

        [HttpPost("add-new-course")]
        [Authorize(Roles ="Instructor")]
        public async Task<IActionResult> AddNewCourse([FromForm] addCourseRequestDto courseRequestDto)
        {
            if (ModelState.IsValid)
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
                    var result = await _courseService.addCourse(courseRequestDto, token);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Message = ex.Message });
                }
            }

            return BadRequest("Invalid Data Request!");
        }


        //get Course info by id

        //Det All Cources

        //get Courses by search



        //delete course

        //update course



    }
}
