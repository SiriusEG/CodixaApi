using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.SearchDtos
{
    public class SearchCoursesResopnseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseCardPhoto { get; set; }
        public string CategoryName { get; set; }
    }
}
