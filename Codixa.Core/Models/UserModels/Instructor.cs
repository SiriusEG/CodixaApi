using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codixa.Core.Models.CourseModels;

namespace Codixa.Core.Models.UserModels
{
    public class Instructor 
    {
        [Key]
        public int InstructorId { get; set; }
        public string InstructorFullName { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public ICollection<CourseRequest> ReviewedRequests { get; set; }

    }
}
