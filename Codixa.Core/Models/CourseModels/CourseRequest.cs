using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.CourseModels
{
    public class CourseRequest
    {
        [Key]
        public int RequestId { get; set; } // Primary Key
        public int StudentId { get; set; } // Foreign Key
        public int CourseId { get; set; } // Foreign Key
        public string RequestStatus { get; set; } = "Pending";// Pending, Approved, Rejected
        public DateTime RequestDate { get; set; } 
        public DateTime? ReviewDate { get; set; }
        public int? ReviewedBy { get; set; } // Teacher ID (Nullable)

        // Navigation Properties
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(ReviewedBy))]
        public virtual Instructor ReviewedByInstructor { get; set; }
    }
}
