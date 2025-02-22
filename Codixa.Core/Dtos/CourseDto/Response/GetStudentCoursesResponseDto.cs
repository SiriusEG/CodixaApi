
using Codixa.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Dtos.CourseDto.Response
{
    public class GetStudentCoursesResponseDto:IKeylessEntity
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public string CourseDescription { get; set; }
        public string CoursePhoto { get; set; }
        public string CategoryName { get; set; }

    }
}
