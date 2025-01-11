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
