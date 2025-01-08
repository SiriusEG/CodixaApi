using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.CourseModels
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }  
        public string CommentText { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }


        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
