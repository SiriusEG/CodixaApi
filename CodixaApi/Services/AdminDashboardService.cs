using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Codixa.Core.Dtos.SearchDtos;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace CodixaApi.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ReturnAllInstructorsReqDto> GetAllInstructors(int pagesize, int pagenumber)
        {
            ReturnAllInstructorsReqDto ReturnAllInstructorsReqDto = null;
            try
            {
                string jsonData = await _unitOfWork.ExecuteStoredProcedureAsStringAsync(
                "ShowAllInstructorRequest",
                    "@PageSize", pagesize,
                    "@PageNumber", pagenumber
                );

                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Deserialize JSON if needed
                    ReturnAllInstructorsReqDto = JsonSerializer.Deserialize<ReturnAllInstructorsReqDto>(jsonData);

                    // Process the response
                    return ReturnAllInstructorsReqDto;
                }
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Error while deserializing JSON data.", jsonEx);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error occurred.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }

            return ReturnAllInstructorsReqDto;
        }

        public async Task<int> ChangeInstructorRequestStatus(ChangeInstructorRequestStatusDto requestStatusDto)
        {
 
            // Execute the stored procedure
            int rowsAffected = await _unitOfWork.ExecuteStoredProcedureAsyncIntReturn(
                "ChangeInstructorRequestStatus @RequestId, @NewStatus",
                new SqlParameter("@RequestId", requestStatusDto.RequestId),
                new SqlParameter("@NewStatus", requestStatusDto.NewStatus.ToString())
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
