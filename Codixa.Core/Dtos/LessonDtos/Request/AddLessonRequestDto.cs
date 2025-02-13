using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.LessonDtos.Request
{
    public class AddLessonRequestDto
    {
        public string LessonName { get; set; }
        public bool IsVideo { get; set; }
        public IFormFile? Video { get; set; }
        public string? LessonText { get; set; }
        public int LessonOrder { get; set; }
        public bool IsForpreview { get; set; }
        public int SectionId { get; set; }


    }
}
