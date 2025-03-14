using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class ChoicesQuestion
    {
        [Key]
        public int ChoicesQuestionId { get; set; }

        public string ChoicesQuestionText { get; set; }
        public bool IsTrue { get; set; }


        public int QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }
    }
}
