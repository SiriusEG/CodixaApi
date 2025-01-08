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
    public class UserAnswer
    {
        [Key]
        public int UserAnswerId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }


        [ForeignKey("ChoicesQuestion")]
        public int SelectedChoicesQuestionId { get; set; }
        public virtual ChoicesQuestion ChoicesQuestion { get; set; }
    }
}
