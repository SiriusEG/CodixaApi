using Codixa.Core.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }

        public decimal Result { get; set; }

        public bool IsPassed { get; set; }



        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }



        [ForeignKey("SectionTest")]
        public int SectionTestId { get; set; }
        public virtual SectionTest SectionTest { get; set; }

        public virtual Certification Certification { get; set; }

    }
}
