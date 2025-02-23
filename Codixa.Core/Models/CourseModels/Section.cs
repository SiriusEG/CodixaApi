using Codixa.Core.Models.SectionsTestsModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.CourseModels
{
    public class Section
    {
        [Key]
        public int SectionId { get; set; }
        public string SectionName { get; set; }

        public int SectionOrder { get; set; }

        public bool IsFinished { get; set; }

        public bool IsOpened { get; set; }
        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public virtual SectionTest SectionTest { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
