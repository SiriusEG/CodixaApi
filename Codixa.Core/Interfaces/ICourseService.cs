using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.CourseDto.Response;

namespace Codixa.Core.Interfaces
{
    public interface ICourseService
    {


        Task<addCourseResponseDto> addCourse(addCourseRequestDto courseRequestDto, string token);
        Task<CourseGetResponseDto> GetCourseById(int CourseId);
        Task<string> DeleteCourse(int CourseId);
        Task<string> UpdateCourse(int CourseId, UpdateCourseRequestDto courseRequestDto);
        Task<(List<GetAllCoursesDetailsResponseDto> Courses, int PageCount)> GetUserCourses(string token, int PageNumber, int PageSize);

    }
}
