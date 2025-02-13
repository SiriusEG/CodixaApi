using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.CourseDto.Response;
using Codixa.Core.Interfaces;
using Codxia.Core;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.UserModels;
using Codixa.Core.Dtos.LessonDtos.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Codixa.Core.Custom_Exceptions;

namespace CodixaApi.Services
{
    public class CourseService: ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IAuthenticationService _authenticationService;
        public CourseService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

      
       public async Task<addCourseResponseDto> addCourse(addCourseRequestDto courseRequestDto , string token)
        {
            var file = await _unitOfWork.Files.UploadFileAsync(courseRequestDto.CourseCardPhoto, Path.Combine("uploads", "Courses-Card-Photos"));

            try
            { 
                    string UserID = await _authenticationService.GetUserIdFromToken(token);

                    Instructor instructor = await _unitOfWork.Instructors.FirstOrDefaultAsync(x=>x.UserId == UserID);

                    await _unitOfWork.Files.AddAsync(file);

                     var Course = await _unitOfWork.Courses.AddAsync(new Course { 
                        CourseName = courseRequestDto.CourseName,
                        CourseDescription = courseRequestDto.CourseDescription,
                        CourseCardPhotoId = file.FileId,
                        IsPublished = false,
                        InstructorId = instructor.InstructorId,
                        CategoryId=courseRequestDto.CategoryId
                    });
                   
                    await _unitOfWork.Complete();

                    return new addCourseResponseDto
                    {
                        CourseName= Course.CourseName,
                        CourseDescription= Course.CourseDescription,
                        CourseCardPhotoFilePath= file.FilePath,
                        CategoryId = Course.CategoryId
                    };
            }
            catch (Exception ex) 
            {
               await _unitOfWork.Files.DeleteExistsFileAsync(file.FilePath);
                throw new Exception("there are error while saving Course Info " + ex.Message);
              
            }
           // return new addCourseResponseDto();

        }


        //get user courses
        public async Task<List<GetAllCoursesDetailsResponseDto>> GetUserCourses(string token)
        {

            try
            {
                var UserID = await _authenticationService.GetUserIdFromToken(token);
                if (UserID != null) {
                    var result = await _unitOfWork.ExecuteStoredProcedureAsync<GetAllCoursesDetailsResponseDto>("GetCoursesByUserId @UserId", new SqlParameter("@UserId", UserID));
                    return result;
                }
                throw new UserNotFoundException("Login To Get Your Courses!");
            }
            catch (UserNotFoundException) {
                throw;
            }


        }
        //updateCourseDetails



            //delete course

        public async Task<string> DeleteCourse(int CourseId)
        {
            // Update Lesson
            try
            {
                try
                {
                    var Course = await _unitOfWork.Courses.FirstOrDefaultAsync(x => x.CourseId == CourseId);
                    if (Course == null)
                    {
                        throw new Exception("Course Not Found!");
                    }
                    await _unitOfWork.Courses.DeleteAsync(Course);
                    await _unitOfWork.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("there are an error while Deleting Course " + ex.Message);
            }



            return "Course Deleted Success";

        }

    }
}
