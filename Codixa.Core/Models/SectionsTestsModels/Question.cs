using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string Text { get; set; }




        public int SectionTestId { get; set; }
        [ForeignKey(nameof(SectionTestId))]
        public virtual SectionTest SectionTest { get; set; }

        public virtual ICollection<ChoicesQuestion> ChoicesQuestions { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
