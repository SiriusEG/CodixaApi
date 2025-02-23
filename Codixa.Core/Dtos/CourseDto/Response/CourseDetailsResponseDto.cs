using Codixa.Core.Dtos.FeedbackDto;
using Codixa.Core.Enums;

namespace Codixa.Core.Dtos.CourseDto.Response
{
    public class CourseDetailsResponseDto
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseCardPhotoPath { get; set; }
        public string CategoryName { get; set; }
        public string InsrtuctorName { get; set; }

        public int SectionCount { get; set; }
        public RateEnum TotalRate { get; set; }
        public int Count5Stars { get; set; }
        public int Count4Stars { get; set; }
        public int Count3Stars { get; set; }
        public int Count2Stars { get; set; }
        public int Count1Star { get; set; }

        public List<FeedBackCourseDto> FeedBacks { get; set; }

    }
}
