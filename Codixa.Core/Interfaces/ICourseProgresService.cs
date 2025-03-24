using Codixa.Core.Dtos.CourseProgressDtos.request;
using Codixa.Core.Dtos.CourseProgressDtos.response;

namespace Codixa.Core.Interfaces
{
    public interface ICourseProgresService
    {
        Task<CourseDataDto> GetCourseContent(int CourseId);
        Task<string> GetLessonDetails(GetLessonDetailsDto getLessonDetailsDto, string token);
    }
}
