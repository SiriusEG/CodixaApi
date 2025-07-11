﻿using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Codixa.Core.Models.SectionsTestsModels
{
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }
        public decimal Result { get; set; }
        public bool IsPassed { get; set; }
        public int AttemptId { get; set; }
        [ForeignKey(nameof(AttemptId))]
        public virtual StudentTestAttempt Attempt { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
        public int SectionTestId { get; set; }
        [ForeignKey(nameof(SectionTestId))]
        public virtual SectionTest SectionTest { get; set; }
        public virtual Certification Certification { get; set; }
    }
}
