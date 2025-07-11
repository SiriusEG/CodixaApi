using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.adminDashDtos.AdminGetUsersDtos
{
    public class GetAllInstructorDto
    {
        [Required]
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePic { get; set; }
        public string? InstructorFullName { get; set; }

        public string? Specialty { get; set; }
    }
}
