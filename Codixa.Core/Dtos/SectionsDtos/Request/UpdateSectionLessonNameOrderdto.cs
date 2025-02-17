using Codixa.Core.Dtos.LessonDtos.Request;
using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.SectionsDtos.Request
{
    public class UpdateSectionLessonNameOrderdto : IKeylessEntity
    {
        public int SectionId { get; set; }
        public int SectionOrder { get; set; }
        public string? SectionName { get; set; }

        public List<updateLessonRequestDto>? Lessons {  get; set; }    
    }
}
