using Codixa.Core.Dtos.CourseDto.CourseDetailsDtos;
using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.CourseDto.Response;
using Codixa.Core.Dtos.SearchDtos;

namespace Codixa.Core.Interfaces
{
    public interface ICourseService
    {


        Task<addCourseResponseDto> addCourse(addCourseRequestDto courseRequestDto, string token);
        Task<CourseGetResponseDto> GetCourseById(int CourseId);
        Task<string> DeleteCourse(int CourseId);
        Task<string> UpdateCourse(int CourseId, UpdateCourseRequestDto courseRequestDto);
        Task<GetAllCoursesDetailsResponseDto> GetUserCourses(string token, int PageNumber, int PageSize);
        Task<SearchCoursesResopnseDto> Search(SearchCoursesDtos searchCoursesDtos, int PageNumber, int PageSize);
        Task<CourseDetailsResponseDto> GetCourseDetailsWithFeedbacksAsync(int courseId);
    }
}
