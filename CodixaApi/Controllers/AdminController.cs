using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodixaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;
        public AdminController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;

        }



        [HttpGet("GetInstructorsRequests/{PageNumber}")]
        public async Task<IActionResult> GetAllInstructorsRequests([FromRoute]int PageNumber)
        {
            var (result, Totalpages) = await _adminDashboardService.GetAllInstructors(10,PageNumber);
            if (result == null&& Totalpages == 0) {
                return BadRequest("No Data Has Found");
            }
            return Ok(new { result , Totalpages });
        }

        [HttpGet("GetApprovedInstructors")]
        public async Task<IActionResult> GetAllApprovedInstructors()
        {

            var result = await _adminDashboardService.GetAllApprovedInstructors();
            if (result == null)
            {
                return BadRequest("No Data Has Found");
            }
            return Ok(result);
        }
            
        [HttpPut("ChangeInstructorStatus")]
        public async Task<IActionResult> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto)
        {
            try
            {
                // تنفيذ العملية
                var result = await _adminDashboardService.ChangeInstructorRequestStatus(requestStatusDto);
                if(result < 1)
                {
                    throw new Exception("there are an error, ");
                }
                // إرجاع النتيجة بنجاح
                return Ok("Instructor Request Updated Successfully");
            }
            catch (RequestIdnotFoundInInstructorJoinRequestsException ex)
            {
                // التعامل مع خطأ محدد (RequestId not found)
                return BadRequest(new { Message = ex.Message });
            }
    
            catch (Exception ex)
            {
                // التعامل مع الأخطاء العامة
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> registerAdmin([FromBody]registerAdminRequestDto registerAdminRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data." + ModelState.Values.SelectMany(e=>e.Errors),
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            // Create the user
            var result = await _adminDashboardService.RegisterAdminAsync(registerAdminRequestDto);

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
                Message = "User registration failed. " + result.Errors,
                
            });
        }

    }
}
