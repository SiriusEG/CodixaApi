using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.CourseProgressDtos.response
{
    public class SectionDto 
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }

      
        public int SectionOrder { get; set; }

     
        public int SectionType { get; set; }


        public List<LessonDto>? SectionContent { get; set; }
        public List<testSectionDto>? SectionTestContent { get; set; }
    }
}
