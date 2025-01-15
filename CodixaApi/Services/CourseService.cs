using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.CourseDto.Response;
using Codixa.Core.Interfaces;
using Codxia.Core;
using Codixa.Core.Models.CourseModels;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Codixa.Core.Models.UserModels;

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
            return new addCourseResponseDto();

        }


    }
}
