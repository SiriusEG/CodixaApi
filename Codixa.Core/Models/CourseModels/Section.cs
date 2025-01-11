using Codixa.Core.Models.SectionsTestsModels;
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
    public class Section
    {
        [Key]
        public int SectionId { get; set; }
        public string SectionName { get; set; }

        public int SectionOrder { get; set; }


        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public virtual SectionTest SectionTest { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
