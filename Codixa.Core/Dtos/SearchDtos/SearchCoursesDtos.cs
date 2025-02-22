using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.SearchDtos
{
    public class SearchCoursesDtos
    {
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public int? CategoryId { get; set; }
    }
}
