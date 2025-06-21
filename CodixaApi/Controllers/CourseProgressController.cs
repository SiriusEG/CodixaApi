using Codixa.Core.Dtos.CourseProgressDtos.request;
using Codixa.Core.Interfaces;
using CodixaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseProgressController : ControllerBase
    {
        private readonly ICourseProgresService _courseProgresService;
        public CourseProgressController(ICourseProgresService courseProgresService)
        {
            _courseProgresService = courseProgresService;

        }
        [HttpGet("GetCourseContent/{CourseId}")]
        [Authorize(Roles = "Student")]

        public async Task<IActionResult> GetCourseContent([FromRoute] int CourseId)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var CourseContent = await _courseProgresService.GetCourseContent(CourseId);
                    return Ok(CourseContent);
                }
                return BadRequest(new { Message = "Enter Course Number" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("GetLessonTestDetails")]
        [Authorize(Roles = "Student")]

        public async Task<IActionResult> GetLessonTestDetails(GetLessonDetailsDto getLessonDetailsDto)
        {
            try
            {
                if (ModelState.IsValid)
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
                    var CourseContent = await _courseProgresService.GetLessonDetails(getLessonDetailsDto, token);
                    return Ok(CourseContent);
                }
                return BadRequest(new { Message = "section or lesson data invalid" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("MarkLessonAsCompleted/{LeesonId}")]
        [Authorize(Roles = "Student")]

        public async Task<IActionResult> MarkLessonAsCompleted([FromRoute] int LeesonId)
        {

            try
            {
                if (ModelState.IsValid)
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
                    var MarkedAsCompleted = await _courseProgresService.MarkLessonAsCompleted(LeesonId, token);
                    return Ok(MarkedAsCompleted);
                }
                return BadRequest(new { Message = "Enter Leeson Id" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
