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

        public DateTime EnrollmentDate { get; set; }


        public int StudentId { get; set; }

        public int CourseId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
    }
}
