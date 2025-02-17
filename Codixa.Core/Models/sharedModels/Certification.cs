using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.sharedModels
{
    public class Certification
    {
        [Key]
        public int CertificationId { get; set; }




        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]

        public virtual Student Student { get; set; }


        public int TestResultId { get; set; }
        [ForeignKey(nameof(TestResultId))]

        public virtual TestResult TestResult { get; set; }
    }
}
