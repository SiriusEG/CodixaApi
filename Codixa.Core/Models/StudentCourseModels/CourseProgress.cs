using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.StudentCourseModels
{
    public class CourseProgress
    {
        [Key]
        public int ProgressId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }
        public double ProgressPercentage { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
    }
}
