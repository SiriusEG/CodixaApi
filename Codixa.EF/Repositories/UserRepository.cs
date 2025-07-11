using Codixa.Core.Models.UserModels;
using Codxia.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Codxia.EF.Repositories
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public UserRepository(UserManager<AppUser> userManager, AppDbContext context) : base(context)
        {
            _userManager = userManager;
            _context = context;
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
        public async Task<IEnumerable<AppUser>> GetAllStudentsAsync()
        {
            return await _userManager.Users.Where(u => u.Student != null).Include(x => x.Student).Include(x => x.Photo).ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAllInstructorsAsync()
        {
            return await _userManager.Users.Where(u => u.Instructor != null && u.InstructorJoinRequests !=null && u.InstructorJoinRequests.Status== "Accepted").Include(x => x.Instructor).Include(x=>x.Photo).Include(x=>x.InstructorJoinRequests).ToListAsync();
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


        public async Task<bool> ChangePasswordWithOldAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordWithoutOldAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

            return result.Succeeded;
        }

    }
}
