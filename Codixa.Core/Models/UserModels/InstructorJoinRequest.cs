using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codixa.Core.Models.sharedModels;

namespace Codixa.Core.Models.UserModels
{
    public class InstructorJoinRequest
    {
        [Key]

        public int RequestId { get; set; }

       
        public string FullName { get; set; }

        public string Specialty { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }

        public string Status { get; set; } = "Pending"; // Status of the request (Pending, Approved, Rejected)

        public DateTime SubmittedAt { get; set; } 

        public string? AdminRemarks { get; set; }

        public string CvFileId { get; set; }
        [ForeignKey(nameof(CvFileId))]
        public virtual FileEntity cv { get; set; }
    }
}
