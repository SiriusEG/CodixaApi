﻿using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.CourseModels
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        public string LessonName { get; set; }

        public bool IsVideo { get; set; }
        public string? VideoId { get; set; }

        public string? LessonText { get; set; }

        public int LessonOrder { get; set; }
        public bool IsForpreview { get; set; } = false;


        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }
        [ForeignKey(nameof(VideoId))]
        public virtual FileEntity Video { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
