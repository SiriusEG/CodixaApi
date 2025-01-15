using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.CourseDto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Interfaces
{
    public interface ICourseService
    {


        Task<addCourseResponseDto> addCourse(addCourseRequestDto courseRequestDto, string token);



    }
}
