using Codixa.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Codixa.Core.Dtos.CourseDto.Request
{
    public class addCourseRequestDto
    {
       
        public string CourseName { get; set; }

        public string CourseDescription { get; set; }

        public int CategoryId { get; set; }

        public IFormFile CourseCardPhoto { get; set; }

        public CourseLevelEnum Level { get; set; }

        public CourseLangugeEnum Language { get; set; }

    }
}
