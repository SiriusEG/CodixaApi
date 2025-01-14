using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface IAdminDashboardService
    {
       Task<List<ReturnAllInstructorsReqDto>> GetAllInstructors();
       Task<int> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto);
        Task<List<ReturnAllApprovedInstructorsDto>> GetAllApprovedInstructors();
    }
}
