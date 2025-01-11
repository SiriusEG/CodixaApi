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




        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }




        public int SectionTestId { get; set; }
        [ForeignKey(nameof(SectionTestId))]
        public virtual SectionTest SectionTest { get; set; }

        public virtual Certification Certification { get; set; }

    }
}
