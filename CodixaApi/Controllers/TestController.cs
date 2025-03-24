using Codixa.Core.Dtos.QuestionsDtos;
using Codixa.Core.Interfaces;
using CodixaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _TestService;
        public TestController(ITestService testService)
        {
            _TestService = testService;

        }



        [HttpPost("AddAnswer")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddAnswer(List<QestionsAnswerDto> qestionsAnswerDto)
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
                    var result = await _TestService.AddAsnwers(qestionsAnswerDto, token);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { Message = "Enter Data Not Valid" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

        }
    }
}
