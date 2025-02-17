using Codixa.Core.Models.CourseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class SectionTest
    {
        [Key]
        public int SectionTestId { get; set; }


        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

    }
}
