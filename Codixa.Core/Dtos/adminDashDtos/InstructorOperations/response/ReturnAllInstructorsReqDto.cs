using Codixa.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response
{
    public class ReturnAllInstructorsReqDto : IKeylessEntity
    {
        public int RequestId { get; set; }
        public string UserName { get; set; } 
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string Status { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string Email { get; set; }
      

        public DateTime DateOfBirth { get; set; }
    
        public string? AdminRemarks { get; set; }
        public string Gender { get; set; }
        public string FilePath { get; set; }
    }
}
