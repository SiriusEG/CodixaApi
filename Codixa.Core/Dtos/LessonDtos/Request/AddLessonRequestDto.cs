using Microsoft.AspNetCore.Http;

namespace Codixa.Core.Dtos.LessonDtos.Request
{
    public class AddLessonRequestDto
    {
        public string LessonName { get; set; }
        public bool IsVideo { get; set; }
        public IFormFile? Video { get; set; } = null;
        public string? LessonText { get; set; }
        public int LessonOrder { get; set; }
        public bool IsForpreview { get; set; }
        public int SectionId { get; set; }


    }
}
