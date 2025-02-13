using Codixa.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.LessonDtos.Request
{
    public class updateLessonRequestDto : IKeylessEntity
    {
        public int? LessonId { get; set; }
        public int SectionId { get; set; }
        public int? LessonOrder { get; set; }
        public string? LessonName { get; set; }
    }
}
