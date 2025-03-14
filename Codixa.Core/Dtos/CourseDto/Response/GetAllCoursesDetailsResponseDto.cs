using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.CourseDto.Response
{
    public class GetAllCoursesDetailsResponseDto : IKeylessEntity
    {

        public List<CoursesDetailsDto> Courses { get; set; }
        public int TotalPages { get; set; }
    }
}
