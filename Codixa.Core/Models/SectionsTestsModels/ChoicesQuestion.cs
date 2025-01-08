using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class ChoicesQuestion
    {
        [Key]
        public int ChoicesQuestionId { get; set; }

        public string ChoicesQuestionText { get; set; }
        public bool IsTrue { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
