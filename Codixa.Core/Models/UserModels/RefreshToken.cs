using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.UserModels
{
    public class RefreshToken
    {
        public RefreshToken() { 
        
            Id = Guid.NewGuid().ToString();
        
        }



        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public virtual AppUser User { get; set; }

        public string refreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRevoked { get; set; }

    }
}
