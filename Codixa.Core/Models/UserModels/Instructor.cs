using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.sharedModels;

namespace Codixa.Core.Models.UserModels
{
    public class Instructor 
    {
        [Key]
        public int InstructorId { get; set; }
        public string InstructorFullName { get; set; }

        public string Specialty { get; set; }

        public string CvFileId { get; set; }

       
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }
        [ForeignKey(nameof(CvFileId))]
        public virtual FileEntity cv { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<CourseRequest> ReviewedRequests { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; } = new List<Certification>();


    }
}
