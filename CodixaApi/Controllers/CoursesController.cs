using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.SearchDtos;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("GetCourseById/{CourseId}")]
        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> GetCourseById([FromRoute] int CourseId)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var Course = await _courseService.GetCourseById(CourseId);
                    return Ok(Course);
                }
                return BadRequest(new { Message = "Enter Course Number" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }




        [HttpGet("GetCoursesByUserToken/{PageNumber}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetUserCourses([FromRoute]int PageNumber) {


            try
            {
                if (ModelState.IsValid)
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
                    var (Courses, totalPages) = await _courseService.GetUserCourses(token, PageNumber, 6);
                    return Ok(new { Courses, totalPages });
                }
                else
                {
                    return BadRequest(new { Message = "Enter Page Number" });
                }
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
                if (!ModelState.IsValid)
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
        [HttpPut("Update/{CourseId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int CourseId,[FromForm] UpdateCourseRequestDto updateCourseRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Course data Is Empty");
                }
                var result = await _courseService.UpdateCourse(CourseId,updateCourseRequestDto);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //get Course info by id




        //get All Cources

        //get course details


        //get Courses by search
        [HttpPost("Search/{PageNumber}")]
       
        public async Task<IActionResult> Search(SearchCoursesDtos searchCoursesDtos, [FromRoute] int PageNumber)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Pass the token and DTO to the service class
                    var (Courses, totalPages) = await _courseService.Search(searchCoursesDtos, PageNumber, 6);
                    return Ok(new { Courses, totalPages });
                }
                else
                {
                    return BadRequest(new { Message = "Enter Page Number" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}
