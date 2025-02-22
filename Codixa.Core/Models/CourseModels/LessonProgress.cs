using Codixa.Core.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.CourseModels
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
