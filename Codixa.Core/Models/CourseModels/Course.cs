using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codixa.Core.Models.UserModels;

namespace Codixa.Core.Models.CourseModels
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }


        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Section>? Sections { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<courseFeedback> courseFeedbacks { get; set; }

    }
}
