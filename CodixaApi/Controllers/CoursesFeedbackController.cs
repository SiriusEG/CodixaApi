using Codixa.Core.Dtos.FeedbackDto;
using Codixa.Core.Interfaces;
using CodixaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesFeedbackController : ControllerBase
    {
        private readonly ICourseFeedbackService _courseFeedbackService;

        public CoursesFeedbackController(ICourseFeedbackService courseFeedbackService )
        {
            _courseFeedbackService = courseFeedbackService;
        }

        //addfeedback
        [HttpPost("Add/{CourseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddFeedBack([FromRoute] int CourseId,[FromBody] FeedBackDto feedBackRequest)
        {
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
                    var result = await _courseFeedbackService.AddFeedback(feedBackRequest,CourseId,token);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { Message = "Missing FeedBack" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }



        //deletefeedback
        [HttpDelete("Delete/{CourseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int CourseId)
        {


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
                    var result = await _courseFeedbackService.Delete(CourseId, token);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { Message = "Missing FeedBack" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
        //updatefeedback

        [HttpPut("Update/{CourseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UpdateFeedback([FromRoute] int CourseId, FeedBackDto feedBackRequest)
        {


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
                    var result = await _courseFeedbackService.Update(feedBackRequest, CourseId, token);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { Message = "Missing FeedBack" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }


    }
}
