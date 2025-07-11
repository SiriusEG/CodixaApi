using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.StudentCourseModels;

namespace Codixa.Core.Models.UserModels
{
    public class Student 
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<courseFeedback> courseFeedbacks { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
        public virtual ICollection<TestResult> TestResults { get; set; }
        public virtual ICollection<CourseRequest> CourseRequests { get; set; }
        public virtual ICollection<CourseProgress> StudentProgresses { get; set; }


    }
}
