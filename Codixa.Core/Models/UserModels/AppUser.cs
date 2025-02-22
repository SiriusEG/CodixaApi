using Microsoft.AspNetCore.Identity;

namespace Codixa.Core.Models.UserModels
{
    public class AppUser : IdentityUser
    {
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual Student Student { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
