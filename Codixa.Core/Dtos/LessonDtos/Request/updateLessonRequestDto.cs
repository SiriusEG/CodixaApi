using Codixa.Core.Interfaces;

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
