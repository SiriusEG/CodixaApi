using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.CourseDto.Request
{
    public class UpdateCourseRequestDto
    {
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }

        public bool? IsPublished { get; set; }

        public IFormFile CourseCardPhoto { get; set; }

        public int? CategoryId { get; set; }

    }
}
