using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Codixa.Core.Models.CourseModels;

namespace Codixa.Core.Models.StudentCourseModels
{
    public class LessonProgress
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        public int LessonId { get; set; }
        [ForeignKey(nameof(LessonId))]
        public virtual Lesson Lesson { get; set; }

        public bool IsCompleted { get; set; }

    }
}
