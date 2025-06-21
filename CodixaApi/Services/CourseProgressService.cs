using Codixa.Core.Dtos.CourseDto.Response;
using Codixa.Core.Dtos.CourseProgressDtos.request;
using Codixa.Core.Dtos.CourseProgressDtos.response;
using Codixa.Core.Dtos.QuestionsDtos;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.SectionsTestsModels;
using Codxia.Core;
using Codxia.EF;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace CodixaApi.Services
{
    public class CourseProgressService: ICourseProgresService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IAuthenticationService _authenticationService;
        public CourseProgressService(IUnitOfWork unitOfWork,IAuthenticationService authenticationService)
        {
            _UnitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

      

        //course data
        public async Task<CourseDataDto> GetCourseContent(int CourseId)
        {
            CourseDataDto courseDataDto = null;

            try
            {
                // Get the User ID from the token


                // Prepare parameters for the stored procedure
                var jsonData = await _UnitOfWork.ExecuteStoredProcedureAsStringAsync(
                    "GetCourseContent",
                    "@CourseId", CourseId

                );
              
                // Check if JSON data is valid
                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Deserialize JSON into the response DTO
                    courseDataDto = JsonSerializer.Deserialize<CourseDataDto>(jsonData);
                }
                else
                {
                    throw new InvalidOperationException("The returned JSON is null or empty.");
                }
            }
            catch (JsonException jsonEx)
            {
                throw new Exception("Error while deserializing JSON data.", jsonEx);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error occurred.", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }

            return courseDataDto;
        }

        //get current lesson

        //move to next lesson

        public async Task<string> GetLessonDetails(GetLessonDetailsDto getLessonDetailsDto,string token )
        {
            try
            {
                var UserId = await _authenticationService.GetUserIdFromToken(token);
                var student = await _UnitOfWork.Students.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (student == null)
                {
                    throw new Exception("Missing Authorization");
                }
                var jsonData = "";
                if (getLessonDetailsDto.LessonId != 0 && getLessonDetailsDto.LessonId!= null)
                {
                    jsonData = await _UnitOfWork.ExecuteStoredProcedureAsStringAsync(
                   "GetLessonOrTestResult",
                   "@StudentId", student.StudentId,
                   "@SectionId", getLessonDetailsDto.SectionId,
                   "@LessonId", getLessonDetailsDto.LessonId);
                }
                else
                {
                    jsonData = await _UnitOfWork.ExecuteStoredProcedureAsStringAsync(
                    "GetLessonOrTestResult",
                    "@StudentId", student.StudentId,
                    "@SectionId", getLessonDetailsDto.SectionId);
                }
                return jsonData;
            }
            catch (Exception ex)
            {
                throw;
            }
         
        }

 
        public async Task<string> MarkLessonAsCompleted (int LessonId,string token)
        {
            try
            {
                var UserId = await _authenticationService.GetUserIdFromToken(token);
             
                if (UserId == null)
                {
                    throw new Exception("Missing Authorization");
                }
                var jsonData = "";
                if (LessonId != 0)
                {
                    jsonData = await _UnitOfWork.ExecuteStoredProcedureAsStringAsync(
                   "MarkLessonAsCompleted",
                   "@LessonId", LessonId,
                   "@UserId", UserId);
                }
       
                return "Done";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
