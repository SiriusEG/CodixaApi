using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.AccountDtos.Request
{
    public class RegisterUserDto
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }
    }
}
