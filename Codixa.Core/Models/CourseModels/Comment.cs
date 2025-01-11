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



        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }


  
        public int LessonId { get; set; }
        [ForeignKey(nameof(LessonId))]
        public virtual Lesson Lesson { get; set; }
    }
}
