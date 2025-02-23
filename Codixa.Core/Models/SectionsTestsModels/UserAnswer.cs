using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class UserAnswer
    {
        [Key]
        public int UserAnswerId { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }
        public int SelectedChoicesQuestionId { get; set; }
        [ForeignKey(nameof(SelectedChoicesQuestionId))]
        public virtual ChoicesQuestion ChoicesQuestion { get; set; }
    }
}
