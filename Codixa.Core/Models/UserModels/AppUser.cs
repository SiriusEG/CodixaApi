using Codixa.Core.Models.sharedModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.UserModels
{
    public class AppUser : IdentityUser
    {
        public bool Gender { get; set; }
  
        public DateTime DateOfBirth { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual Student Student { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual InstructorJoinRequest InstructorJoinRequests { get; set; }

        public string? PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public FileEntity?  Photo { get; set; }


    }
}
