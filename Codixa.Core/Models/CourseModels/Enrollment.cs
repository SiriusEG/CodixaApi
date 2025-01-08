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
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }


        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}
