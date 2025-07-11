using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos
{
    public class GetUpdateStudentsDto
    {
        [Required]
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ProfilePic { get; set; }
        public string? StudentFullName { get; set; }


    }
}
