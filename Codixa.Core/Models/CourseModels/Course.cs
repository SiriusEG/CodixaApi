using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;

namespace Codixa.Core.Models.CourseModels
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        public bool IsPublished { get; set; }

        public string CourseCardPhotoId { get; set; }


        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }
    
        public int InstructorId { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public virtual ICollection<Section>? Sections { get; set; }
        [ForeignKey(nameof(InstructorId))]
        public virtual Instructor Instructor { get; set; }

        public virtual ICollection<courseFeedback> courseFeedbacks { get; set; }

        public virtual ICollection<CourseRequest> CourseRequests { get; set; }

        [ForeignKey(nameof(CourseCardPhotoId))]
        public virtual FileEntity Photo { get; set; }

        public virtual ICollection<CourseProgress> StudentProgresses { get; set; }


    }
}
