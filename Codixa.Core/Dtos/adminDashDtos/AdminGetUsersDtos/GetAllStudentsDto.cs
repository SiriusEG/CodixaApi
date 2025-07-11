using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos
{
    public class GetAllStudentsDto
    {
        [Required]
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePic { get; set; }
        public string? StudentFullName { get; set; }


    }
}
