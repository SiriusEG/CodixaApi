using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Dtos.AccountDtos.Request
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
