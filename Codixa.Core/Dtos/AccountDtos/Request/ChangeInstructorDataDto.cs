using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Dtos.AccountDtos.Request
{
    public class ChangeInstructorDataDto
    {
        [Required]
        public string Password { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ProfilePic { get; set; }
        public string? InstructorFullName { get; set; }
        public string? Specialty { get; set; }

    }
}
