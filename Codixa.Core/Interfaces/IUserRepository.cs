using Codxia.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<AppUser> FindByNameAsync(string username);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<AppUser> FindByIdAsync(string userId);
        Task<IList<string>> GetRolesAsync(AppUser user);
        Task<IdentityResult> AddToRoleAsync(AppUser user, string Role);
        Task<IdentityResult> DeleteAsync(AppUser User);

        Task<IdentityResult> ChangePasswordAsync(AppUser User, string newPassword);
        Task<IdentityResult> ChangeUserNameAsync(AppUser user, string newUserName);

        Task<IEnumerable<AppUser>> GetAllUsersAsync();
    }
}
