using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Dtos.AccountDtos.Request
{
    public class RegisterInstructorDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Specialty { get; set; }
        [Required]
        public IFormFile Cv { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
    }
}
