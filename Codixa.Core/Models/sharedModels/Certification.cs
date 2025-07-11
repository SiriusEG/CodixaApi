using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.sharedModels
{
    public class Certification
    {
        [Key]
        public string CertificationId { get; set; }

        public DateTime CertificationIssueDate { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        public int InstructorId { get; set; }
        [ForeignKey(nameof(InstructorId))]
        public virtual Instructor Instructor { get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public int TestResultId { get; set; }
        [ForeignKey(nameof(TestResultId))]
        public virtual TestResult TestResult { get; set; }

    }
}
