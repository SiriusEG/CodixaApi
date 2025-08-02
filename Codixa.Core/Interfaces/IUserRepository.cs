using Codixa.Core.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Codxia.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<AppUser>
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
        Task<IEnumerable<AppUser>> GetAllInstructorsAsync();
        Task<IEnumerable<AppUser>> GetAllStudentsAsync();
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<bool> ChangePasswordWithoutOldAsync(string userId, string newPassword);
        Task<bool> ChangePasswordWithOldAsync(string userId, string oldPassword, string newPassword);
        Task<IEnumerable<AppUser>> GetUsersInRoleAdmin();

    }
}
