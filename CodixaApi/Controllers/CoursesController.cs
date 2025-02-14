using Azure.Core;
using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.SectionsDtos.Request;
using Codixa.Core.Interfaces;
using CodixaApi.Services;
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

        [HttpGet("GetCoursesByUserToken/{PageNumber}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetUserCourses([FromRoute]int PageNumber) {

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
                var (Courses, totalPages) = await _courseService.GetUserCourses(token,PageNumber,6);
                return Ok(new { Courses, totalPages });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }


        [HttpPost("AddNewCourse")]
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
        //delete course

        [HttpDelete("Delete/{CourseId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteCourse([FromRoute]int CourseId)
        {
            try
            {
                if (ModelState == null)
                {
                    return BadRequest("Course Id Is Empty");
                }
                var result = await _courseService.DeleteCourse(CourseId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //update course
        [HttpPut("Update")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateCourse([FromForm] UpdateCourseRequestDto updateCourseRequestDto)
        {
            try
            {
                if (ModelState == null)
                {
                    return BadRequest("Course data Is Empty");
                }
                var result = await _courseService.UpdateCourse(updateCourseRequestDto);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //get Course info by id




        //Det All Cources

        //get Courses by search

    }
}
