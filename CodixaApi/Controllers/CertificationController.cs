using Codixa.Core.Interfaces;
using CodixaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificationController : ControllerBase
    {
        private readonly ICertificationService _CertificationService;
        public CertificationController(ICertificationService CertificationService)
        {

            _CertificationService = CertificationService;

        }
        [HttpGet("GetCertification/{CourseId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetCertification([FromRoute] int CourseId)
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
                    var result = await _CertificationService.GetCertification(CourseId, token);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { Message = "Model State not valid" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("VerifyCertification/{CertificationId}")]
        public async Task<IActionResult> VerifyCertification([FromRoute] string CertificationId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _CertificationService.VerifyCertification(CertificationId);
                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { Message = "Enter Certification Number" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
