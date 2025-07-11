using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.AccountDtos.Response;
using Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos;
using Microsoft.AspNetCore.Identity;

namespace Codixa.Core.Interfaces
{
    public interface IAuthenticationService
    {

        Task<IdentityResult> RegisterStudentAsync(RegisterStudentDto model);
        Task<IdentityResult> RegisterInstructorAsync(RegisterInstructorDto model);
        Task<LoginTokenDto> LoginAsync(LoginUserDto model);

        Task<LoginTokenDto> RefreshToken(LoginTokenDto model);

        Task<string> GetUserIdFromToken(string token);

        Task<bool> ChangeUserPassword(string Token, ChangeUserWithConfirmDto changeUserWithConfirmDto);
        Task<GetAllStudentsDto> ChangeStudentData(string Token, ChangeStudentDataDto changeStudentDataDto);
        Task<GetAllInstructorDto> ChangeInstructorData(string Token, ChangeInstructorDataDto changeInstructorData);
        Task<GetAllStudentsDto> GetStudentData(string Token);
        Task<GetAllInstructorDto> GetInstructorData(string Token);



    }


}
