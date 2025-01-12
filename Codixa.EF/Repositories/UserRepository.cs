using Codixa.Core.Models.UserModels;
using Codxia.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<AppUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser user, string Role)
        {
            return await _userManager.AddToRoleAsync(user, Role);
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }


        public async Task<IdentityResult> DeleteAsync(AppUser User)
        {
            return await _userManager.DeleteAsync(User);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }


        public async Task<IdentityResult> ChangePasswordAsync(AppUser User, string newPassword)
        {

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(User);

            // Reset the password using the generated token
            return await _userManager.ResetPasswordAsync(User, resetToken, newPassword);

        }

        public async Task<IdentityResult> ChangeUserNameAsync(AppUser user, string newUserName)
        {
            // Change the user's username
            return await _userManager.SetUserNameAsync(user, newUserName);
        }

 
    }
}
