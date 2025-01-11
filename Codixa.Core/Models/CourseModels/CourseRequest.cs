using Codixa.Core.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.CourseModels
{
    public class CourseRequest
    {
        [Key]
        public int RequestId { get; set; } // Primary Key
        public int StudentId { get; set; } // Foreign Key
        public int CourseId { get; set; } // Foreign Key
        public string RequestStatus { get; set; } // Pending, Approved, Rejected
        public DateTime RequestDate { get; set; } 
        public DateTime? ReviewDate { get; set; }
        public int? ReviewedBy { get; set; } // Teacher ID (Nullable)

        // Navigation Properties
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
        [ForeignKey(nameof(ReviewedBy))]
        public Instructor ReviewedByInstructor { get; set; }
    }
}
