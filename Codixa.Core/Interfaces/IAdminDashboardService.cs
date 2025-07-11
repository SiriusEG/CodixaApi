using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Microsoft.AspNetCore.Identity;

namespace Codixa.Core.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<ReturnAllInstructorsReqDto> GetAllInstructors(int pagesize, int pagenumber, string SearchTearm = null);
        Task<int> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto);
        Task<List<ReturnAllApprovedInstructorsDto>> GetAllApprovedInstructors();

        Task<IdentityResult> RegisterAdminAsync(registerAdminRequestDto registerAdminRequestDto);

        Task<List<GetAllStudentsDto>> GetAllStudents();
        Task<List<GetAllInstructorDto>> GetAllInstructors();
        Task<GetAllStudentsDto> changeStudentData(GetUpdateStudentsDto getAllStudentsDto);
        Task<GetAllInstructorDto> changeInstructorData(GetUpdateInstructorDto getAllInstructorDto);
        Task<string> changePassword(PasswordChangeDto passwordChangeDto);
    }
}
