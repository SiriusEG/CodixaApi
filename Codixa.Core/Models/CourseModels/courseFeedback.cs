using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Codixa.Core.Enums;
using Codixa.Core.Models.UserModels;

namespace Codixa.Core.Models.CourseModels
{
    public class courseFeedback
    {
        [Key]
        public int FeedBackId { get; set; }
        public string? Comment { get; set; }
        public RateEnum rate { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
    }
}
