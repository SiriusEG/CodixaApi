using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Codixa.Core.Interfaces
{
    public interface IAuthenticationService
    {

        Task<IdentityResult> RegisterStudentAsync(RegisterStudentDto model);
        Task<IdentityResult> RegisterInstructorAsync(RegisterInstructorDto model);
        Task<string> LoginAsync(LoginUserDto model);




    }


}
