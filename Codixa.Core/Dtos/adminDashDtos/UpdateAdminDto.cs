using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Dtos.adminDashDtos
{
    public class UpdateAdminDto
    {
        [Required]
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? NewPhoto { get; set; }
    }
}
