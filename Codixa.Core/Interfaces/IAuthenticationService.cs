using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.AccountDtos.Response;
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

       
    }


}
