using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IdentityResult> RegisterAdminAsync(registerAdminRequestDto registerAdminRequestDto)
        {
           
            var user = new AppUser
            {
                UserName = registerAdminRequestDto.UserName,
                Email = registerAdminRequestDto.Email,
                PhoneNumber = registerAdminRequestDto.PhoneNumber,
                Gender = registerAdminRequestDto.Gender,
                DateOfBirth = registerAdminRequestDto.DateOfBirth
            };



            var result = await _unitOfWork.UsersManger.CreateAsync(user, registerAdminRequestDto.Password);


            if (result.Succeeded)
            {
                // Assign role to the user
                await _unitOfWork.UsersManger.AddToRoleAsync(user, "Admin");

         
                // Save the changes to the database
                await _unitOfWork.Complete();

               
            }
            return result;
        }

        public Task<List<ReturnAllApprovedInstructorsDto>> GetAllApprovedInstructors()
        {
            
            return _unitOfWork.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors");

        }



    }
}
