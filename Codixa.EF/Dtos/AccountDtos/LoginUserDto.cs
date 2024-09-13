using System.ComponentModel.DataAnnotations;

namespace Codixa.EF.Dtos.AccountDtos
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
