using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.StudentCourseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.CourseModels
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        public string LessonName { get; set; }

        public bool IsVideo { get; set; }
        public string? VideoId { get; set; }

        public string? LessonText { get; set; }

        public int LessonOrder { get; set; }
        public bool IsForpreview { get; set; } = false;


        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }
        [ForeignKey(nameof(VideoId))]
        public virtual FileEntity Video { get; set; }
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
