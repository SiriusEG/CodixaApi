﻿using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Codixa.Core.Models.CourseModels;

namespace Codixa.Core.Models.StudentCourseModels
{
    public class SectionProgress
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public bool IsCompleted { get; set; }
    }
}
