using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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



        [HttpGet("Instructors-Requests")]
        public async Task<IActionResult> GetAllInstructorsRequests()
        {
            var result = await _adminDashboardService.GetAllInstructors();
            if (result == null) {
                return BadRequest("No Data Has Found");
            }
            return Ok(result);
        }

        [HttpGet("Get-Instructor")]
        public async Task<IActionResult> GetAllApprovedInstructors()
        {

            var result = await _adminDashboardService.GetAllApprovedInstructors();
            if (result == null)
            {
                return BadRequest("No Data Has Found");
            }
            return Ok(result);
        }
            
        [HttpPut("Instructor-Status")]
        public async Task<IActionResult> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto)
        {
            try
            {
                // تنفيذ العملية
                var result = await _adminDashboardService.ChangeInstructorRequestStatus(requestStatusDto);

                // إرجاع النتيجة بنجاح
                return Ok(result);
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


        

    }
}
