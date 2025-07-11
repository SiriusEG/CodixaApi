using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class StudentTestAttempt
    {
        [Key]
        public int AttemptId { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        public int SectionTestId { get; set; }
        [ForeignKey(nameof(SectionTestId))]
        public virtual SectionTest SectionTest { get; set; }

        public int AttemptNumber { get; set; }

        public DateTime AttemptDate { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
