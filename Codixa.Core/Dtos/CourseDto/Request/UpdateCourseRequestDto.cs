using Microsoft.AspNetCore.Http;

namespace Codixa.Core.Dtos.CourseDto.Request
{
    public class UpdateCourseRequestDto
    {
       
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }

        public bool? IsPublished { get; set; }

        public IFormFile? CourseCardPhoto { get; set; }

        public int? CategoryId { get; set; }

    }
}
