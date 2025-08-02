using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos;
using Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Codixa.Core.Dtos.SearchDtos;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public async Task<ReturnAllInstructorsReqDto> GetAllInstructors(int pagesize, int pagenumber, string SearchTearm = null)
        {
            ReturnAllInstructorsReqDto ReturnAllInstructorsReqDto = null;
            try
            {
                string jsonData = await _unitOfWork.ExecuteStoredProcedureAsStringAsync(
                "ShowAllInstructorRequest",
                    "@PageSize", pagesize,
                    "@PageNumber", pagenumber,
                    "@SearchTerm", SearchTearm
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
        //getallusers
        public async Task<List<GetAllInstructorDto>> GetAllInstructors()
        {
            try
            {
                var Instructors = await _unitOfWork.UsersManger.GetAllInstructorsAsync();
                List<GetAllInstructorDto> getAllInstructorDto = Instructors.Select(x => new GetAllInstructorDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    Specialty = x.Instructor.Specialty,
                    InstructorFullName = x.Instructor.InstructorFullName,
                    ProfilePic = x.Photo != null ? x.Photo.FilePath : "",
                    PhoneNumber = x.PhoneNumber
                }).ToList();

                return getAllInstructorDto;

            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async Task<List<GetAllStudentsDto>> GetAllStudents()
        {
            try
            {
                var Students = await _unitOfWork.UsersManger.GetAllStudentsAsync();
                List<GetAllStudentsDto> getAllStudentsDto = Students.Select(x => new GetAllStudentsDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    StudentFullName = x.Student.StudentFullName,
                    ProfilePic = x.Photo != null ? x.Photo.FilePath : "",
                    PhoneNumber = x.PhoneNumber
                }).ToList();

                return getAllStudentsDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
        //change Userdata

        public async Task<GetAllInstructorDto> changeInstructorData(GetUpdateInstructorDto getAllInstructorDto)
        {
            try
            {


                var User = await _unitOfWork.UsersManger.FirstOrDefaultAsync(u => u.Id == getAllInstructorDto.Id, x => x.Include(x => x.Instructor), x => x.Include(x => x.Photo));

                if (User == null)
                {
                    throw new Exception("User Not Found");
                }

                if (!string.IsNullOrWhiteSpace(getAllInstructorDto.UserName))
                {
                    await _unitOfWork.UsersManger.ChangeUserNameAsync(User, getAllInstructorDto.UserName);
                }


                if (!string.IsNullOrWhiteSpace(getAllInstructorDto.Email))
                    User.Email = getAllInstructorDto.Email;

                if (!string.IsNullOrWhiteSpace(getAllInstructorDto.Specialty))
                    User.Instructor.Specialty = getAllInstructorDto.Specialty;

                if (!string.IsNullOrWhiteSpace(getAllInstructorDto.InstructorFullName))
                    User.Instructor.InstructorFullName = getAllInstructorDto.InstructorFullName;

                if (!string.IsNullOrWhiteSpace(getAllInstructorDto.PhoneNumber))
                    User.PhoneNumber = getAllInstructorDto.PhoneNumber;

                FileEntity file = new FileEntity();
                if (getAllInstructorDto.ProfilePic != null)
                {
                    if (User.Photo != null)
                    {
                        await _unitOfWork.Files.DeleteAsync(User.Photo);
                    }
                    file = await _unitOfWork.Files.UploadFileAsync(getAllInstructorDto.ProfilePic, Path.Combine("uploads", "UsersPics"));
                    if (file != null)
                    {
                        User.Photo = file;
                    }
                }

                User = await _unitOfWork.UsersManger.UpdateAsync(User);
                await _unitOfWork.Complete();

                return new GetAllInstructorDto()
                {
                    UserName = User.UserName,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    InstructorFullName = User.Instructor.InstructorFullName,
                    Id = User.Id,
                    Specialty = User.Instructor.Specialty,
                    ProfilePic = file?.FilePath
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., invalid token format)
                throw new Exception("Failed Update Instructor Data.", ex);
            }
        }

        public async Task<GetAllStudentsDto> changeStudentData(GetUpdateStudentsDto getAllStudentsDto)
        {
            try
            {

                var User = await _unitOfWork.UsersManger.FirstOrDefaultAsync(u => u.Id == getAllStudentsDto.Id, x => x.Include(x => x.Student), x => x.Include(x => x.Photo));
                if (User == null)
                {
                    throw new Exception("User Not Found");
                }


                if (!string.IsNullOrWhiteSpace(getAllStudentsDto.UserName))
                {
                    await _unitOfWork.UsersManger.ChangeUserNameAsync(User, getAllStudentsDto.UserName);
                }
     

                if (!string.IsNullOrWhiteSpace(getAllStudentsDto.Email))
                    User.Email = getAllStudentsDto.Email;


                if (!string.IsNullOrWhiteSpace(getAllStudentsDto.StudentFullName))
                    User.Student.StudentFullName = getAllStudentsDto.StudentFullName;

                if (!string.IsNullOrWhiteSpace(getAllStudentsDto.PhoneNumber))
                    User.PhoneNumber = getAllStudentsDto.PhoneNumber;

                FileEntity file = new FileEntity();
                if (getAllStudentsDto.ProfilePic != null)
                {
                    if (User.Photo != null)
                    {
                        await _unitOfWork.Files.DeleteAsync(User.Photo);
                    }
                    file = await _unitOfWork.Files.UploadFileAsync(getAllStudentsDto.ProfilePic, Path.Combine("uploads", "UsersPics"));
                    if (file != null)
                    {
                        User.Photo = file;
                    }
                }

                User = await _unitOfWork.UsersManger.UpdateAsync(User);
                await _unitOfWork.Complete();


                return new GetAllStudentsDto { Id = User.Id, Email = User.Email, UserName = User.UserName, PhoneNumber = User.PhoneNumber, ProfilePic = file?.FilePath, StudentFullName = User.Student.StudentFullName };
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., invalid token format)
                throw new Exception("Failed Update Student Data.", ex);
            }
        }

        public async Task<string> changePassword(PasswordChangeDto passwordChangeDto)
        {
            try
            {
                var Result = await _unitOfWork.UsersManger.ChangePasswordWithoutOldAsync(passwordChangeDto.UserId, passwordChangeDto.NewPassword);
                if (Result)
                    return "Password Changed successfully";
                else
                    return "An unexpected error occurred while changing password";
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }


        public async Task<List<AdminGetDto>> GetallAdmins()
        {
            try
            {
                var admins = await _unitOfWork.UsersManger.GetUsersInRoleAdmin();
                List<AdminGetDto> Admins = admins.Select(x => new AdminGetDto
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    PhotoPath = x.Photo != null ? x.Photo.FilePath : "",

                }).ToList();



                return Admins;

            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }


        public async Task<AdminGetDto> UpdateAdminData (UpdateAdminDto updateAdminDto)
        {
            try
            {
                if(updateAdminDto.UserId == null)
                {
                    throw new Exception("Enter UserId");
                }
                var User = await _unitOfWork.UsersManger.FirstOrDefaultAsync(x => x.Id == updateAdminDto.UserId, x => x.Include(x => x.Photo));
                if(User == null)
                {
                    throw new Exception("Cant Find This User");
                }

                if (!string.IsNullOrWhiteSpace(updateAdminDto.Email))
                    User.Email = updateAdminDto.Email;


                if (!string.IsNullOrWhiteSpace(updateAdminDto.UserName))
                {
                    await _unitOfWork.UsersManger.ChangeUserNameAsync(User, updateAdminDto.UserName);
                }



                if (!string.IsNullOrWhiteSpace(updateAdminDto.PhoneNumber))
                    User.PhoneNumber = updateAdminDto.PhoneNumber;

                FileEntity file = new FileEntity();
                if (updateAdminDto.NewPhoto != null)
                {
                    if (User.Photo != null)
                    {
                        await _unitOfWork.Files.DeleteAsync(User.Photo);
                    }
                    file = await _unitOfWork.Files.UploadFileAsync(updateAdminDto.NewPhoto, Path.Combine("uploads", "UsersPics"));
                    if (file != null)
                    {
                        User.Photo = file;
                    }
                }
                User = await _unitOfWork.UsersManger.UpdateAsync(User);
                await _unitOfWork.Complete();

                return new AdminGetDto
                {
                    UserId = User.Id,
                    UserName = User.UserName,
                    PhoneNumber = User.PhoneNumber,
                    Email = User.Email,
                    PhotoPath = User.Photo != null ? User.Photo.FilePath : "",
                };

            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

    }
}
