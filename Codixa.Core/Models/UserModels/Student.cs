using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;

namespace Codixa.Core.Models.UserModels
{
    public class Student 
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<courseFeedback> courseFeedbacks { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
    }
}
