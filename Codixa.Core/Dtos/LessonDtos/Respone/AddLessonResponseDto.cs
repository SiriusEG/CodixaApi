using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.LessonDtos.Respone
{
    public class AddLessonResponseDto
    {
        public string LessonName { get; set; }

        public bool IsVideo { get; set; }
        public string? VideoLink { get; set; }

        public string? LessonText { get; set; }

        public int LessonOrder { get; set; }
        public bool IsForpreview { get; set; }

    }
}
