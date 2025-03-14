using Codixa.Core.Enums;

namespace Codixa.Core.Dtos.CourseDto.Response
{
    public class addCourseResponseDto
    {
        public string CourseName { get; set; }

        public string CourseDescription { get; set; }

        public int CategoryId { get; set; }

        public string CourseCardPhotoFilePath { get; set; }

        public CourseLevelEnum Level { get; set; }

        public CourseLangugeEnum Language { get; set; }
    }
}
