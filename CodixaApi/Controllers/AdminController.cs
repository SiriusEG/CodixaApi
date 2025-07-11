using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize(Roles ="Admin")]
        [HttpPost("GetInstructorsRequests/{PageNumber}")]
        public async Task<IActionResult> GetAllInstructorsRequests([FromRoute] int PageNumber, [FromBody]string SearchTerm = null)
        {
            var result = await _adminDashboardService.GetAllInstructors(10, PageNumber, SearchTerm);

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPut("ChangeInstructorData")]
        public async Task<IActionResult> ChangeInstructorData([FromForm] GetUpdateInstructorDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var updatedInstructor = await _adminDashboardService.changeInstructorData(dto);

                if (updatedInstructor == null)
                {
                    return NotFound(new { Message = "Instructor not found." });
                }

                return Ok(new
                {
                    Message = "Instructor updated successfully.",
                    Data = updatedInstructor
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while updating instructor.",
                    Error = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("ChangeStudentData")]
        public async Task<IActionResult> ChangeStudentData([FromForm] GetUpdateStudentsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Invalid request data.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var updatedStudent = await _adminDashboardService.changeStudentData(dto);

                if (updatedStudent == null)
                {
                    return NotFound(new { Message = "Student not found." });
                }

                return Ok(new
                {
                    Message = "Student updated successfully.",
                    Data = updatedStudent
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while updating student.",
                    Error = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _adminDashboardService.GetAllStudents();

                return Ok(new
                {
                    Message = "Students fetched successfully.",
                    Data = students
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while fetching students.",
                    Error = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllInstructors")]
        public async Task<IActionResult> GetAllInstructors()
        {
            try
            {
                var instructors = await _adminDashboardService.GetAllInstructors();

                return Ok(new
                {
                    Message = "Instructors fetched successfully.",
                    Data = instructors
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while fetching instructors.",
                    Error = ex.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("ChangeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody]PasswordChangeDto passwordChangeDto)
        {
            try
            {
                
                var Result = _adminDashboardService.changePassword(passwordChangeDto);
                return Ok(new
                {
                    Message = "Password Updates successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while fetching instructors.",
                    Error = ex.Message
                });
            }
        }

    }
}
