using Codixa.Core.Dtos.CourseDto.Response;

namespace Codixa.Core.Interfaces
{
    public interface IStudentService
    {
        Task<string> RequestToEnrollCourse(int CourseId, String token);
        Task<List<GetStudentCoursesResponseDto>> GetStudentCoursesByToken(String token);
    }
}
