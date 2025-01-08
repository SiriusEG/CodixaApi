using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string Text { get; set; }



        [ForeignKey("SectionTest")]
        public int SectionTestId { get; set; }
        public virtual SectionTest SectionTest { get; set; }

        public virtual ICollection<ChoicesQuestion> ChoicesQuestions { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
