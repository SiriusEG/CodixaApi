using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Microsoft.AspNetCore.Identity;

namespace Codixa.Core.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<(List<ReturnAllInstructorsReqDto> Instructors, int PageCount)> GetAllInstructors(int pagesize, int pagenumber);
       Task<int> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto);
        Task<List<ReturnAllApprovedInstructorsDto>> GetAllApprovedInstructors();

        Task<IdentityResult> RegisterAdminAsync(registerAdminRequestDto registerAdminRequestDto);
    }
}
