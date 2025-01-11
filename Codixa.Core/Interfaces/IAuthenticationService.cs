using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Codixa.Core.Interfaces
{
    public interface IAuthenticationService
    {

        Task<IdentityResult> RegisterInstructorAsync(RegisterUserDto model);
        Task<string> LoginAsync(LoginUserDto model);




    }


}
