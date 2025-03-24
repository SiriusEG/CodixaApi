using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.CourseProgressDtos.response
{
    public class CourseDataDto
    {
      
        public int? LastLessonId { get; set; }


        public List<SectionDto> CourseData { get; set; }
    }
}
