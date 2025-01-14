using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Codixa.Core.Interfaces;
using Codxia.Core;
using Microsoft.Data.SqlClient;
using System.Collections.Immutable;

namespace CodixaApi.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<List<ReturnAllInstructorsReqDto>> GetAllInstructors()
        {
            return _unitOfWork.ExecuteStoredProcedureAsync<ReturnAllInstructorsReqDto>("ShowAllInstructorRequest");
        }

        public async Task<int> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto)
        {
            // Define the allowed statuses
            var allowedStatuses = new List<string> {"Approved", "Rejected" };

            // Check if the provided status is valid
            if (!allowedStatuses.Contains(requestStatusDto.NewStatus))
            {
                throw new ArgumentException("Invalid status. Allowed values are: Pending, Approved, Rejected.");
            }

            // Execute the stored procedure
            int rowsAffected = await _unitOfWork.ExecuteStoredProcedureAsyncIntReturn(
                "EXEC ChangeInstructorRequestStatus @InstructorId, @NewStatus",
                new SqlParameter("@InstructorId", requestStatusDto.RequestId),
                new SqlParameter("@NewStatus", requestStatusDto.NewStatus)
            );

            // Check if the request ID was found
            if (rowsAffected == -1)
            {
                throw new RequestIdnotFoundInInstructorJoinRequestsException("RequestId not found!");
            }

            return rowsAffected;
        }



        public Task<List<ReturnAllApprovedInstructorsDto>> GetAllApprovedInstructors()
        {
            
            return _unitOfWork.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors");

        }



    }
}
