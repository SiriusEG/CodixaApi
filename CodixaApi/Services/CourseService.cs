using Codixa.Core.Dtos.CourseDto.Request;
using Codixa.Core.Dtos.CourseDto.Response;
using Codixa.Core.Interfaces;
using Codxia.Core;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.SearchDtos;
using System.Data;
using Codxia.EF;
using Codixa.Core.Dtos.CourseDto.CourseDetailsDtos;
using System.Text.Json;


namespace CodixaApi.Services
{
    public class CourseService: ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IAuthenticationService _authenticationService;
        private readonly AppDbContext _Context;

        public CourseService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,AppDbContext appDbContext)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _Context = appDbContext;
        }


        public async Task<CourseGetResponseDto> GetCourseById(int CourseId)
        {

            try
            {
                var Course = await _unitOfWork.Courses.FirstOrDefaultAsync(x=>x.CourseId==CourseId & x.IsDeleted==false,c=>c.Include(x=>x.Photo));

                

                return new CourseGetResponseDto
                {
                    CourseId= Course.CourseId,
                    CourseName = Course.CourseName,
                    CourseDescription = Course.CourseDescription,
                    CourseCardPhotoFilePath = Course.Photo.FilePath,
                    CategoryId = Course.CategoryId,
                    IsPublished = Course.IsPublished,
                    Level = Course.Level,
                    Language = Course.Language

                };
            }
            catch (Exception ex)
            {
               
                throw new Exception("there are no Course With This Id ");

            }
            // return new addCourseResponseDto();

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
                        IsDeleted = false,
                        InstructorId = instructor.InstructorId,
                        CategoryId=courseRequestDto.CategoryId,
                        Level = courseRequestDto.Level,
                        Language = courseRequestDto.Language

                     });
                   
                    await _unitOfWork.Complete();

                    return new addCourseResponseDto
                    {
                        CourseName= Course.CourseName,
                        CourseDescription= Course.CourseDescription,
                        CourseCardPhotoFilePath= file.FilePath,
                        CategoryId = Course.CategoryId,
                        Level = Course.Level,
                        Language = Course.Language
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
        public async Task<GetAllCoursesDetailsResponseDto> GetUserCourses(string token,int PageNumber,int PageSize )
        {
            GetAllCoursesDetailsResponseDto getAllCoursesDetailsResponseDto = null;

            try
            {
                // Get the User ID from the token
                var UserID = await _authenticationService.GetUserIdFromToken(token);

                // Prepare parameters for the stored procedure
                var jsonData = await _unitOfWork.ExecuteStoredProcedureAsStringAsync(
                    "GetCoursesByUserId",
                    "@UserId", UserID,
                    "@PageSize", PageSize,
                    "@PageNumber", PageNumber
                );

                // Check if JSON data is valid
                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Deserialize JSON into the response DTO
                    getAllCoursesDetailsResponseDto = JsonSerializer.Deserialize<GetAllCoursesDetailsResponseDto>(jsonData);
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

            return getAllCoursesDetailsResponseDto;
        }
        //updateCourseDetails

        public async Task<string> UpdateCourse(int CourseId,UpdateCourseRequestDto courseRequestDto)
        {
            try
            {
                // Fetch the existing course
                Course OldCourse = await _unitOfWork.Courses.FirstOrDefaultAsync(
                    x => x.CourseId == CourseId,
                    z => z.Include(x => x.Photo)
                );

                if (OldCourse == null)
                {
                    throw new Exception("Course Not Found!");
                }

                // Update course properties if provided
                if (!string.IsNullOrEmpty(courseRequestDto.CourseName))
                {
                    OldCourse.CourseName = courseRequestDto.CourseName;
                }
                if (!string.IsNullOrEmpty(courseRequestDto.CourseDescription))
                {
                    OldCourse.CourseDescription = courseRequestDto.CourseDescription;
                }
                if (courseRequestDto.IsPublished.HasValue)
                {
                    OldCourse.IsPublished = courseRequestDto.IsPublished.Value;
                }
                if (courseRequestDto.CategoryId.HasValue && courseRequestDto.CategoryId != 0)
                {
                    OldCourse.CategoryId = courseRequestDto.CategoryId.Value;
                }

                if (courseRequestDto.Level.HasValue)
                {
                    OldCourse.Level = courseRequestDto.Level.Value;
                }

                if (courseRequestDto.Language.HasValue)
                {
                    OldCourse.Language = courseRequestDto.Language.Value;
                }

                // Handle file update (delete old file and upload new file)
                if (courseRequestDto.CourseCardPhoto != null && courseRequestDto.CourseCardPhoto.Length != 0)
                {
                    try
                    {
                        // Delete the old file
                        if (OldCourse.Photo != null)
                        {
                            await _unitOfWork.Files.DeleteExistsFileAsync(OldCourse.Photo.FilePath);
                            await _unitOfWork.Files.DeleteAsync(OldCourse.Photo);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error occurred while deleting the old file: " + ex.Message);
                    }

                    try
                    {
                        // Upload the new file
                        OldCourse.Photo = await _unitOfWork.Files.UploadFileAsync(
                            courseRequestDto.CourseCardPhoto,
                            Path.Combine("uploads", "Courses-Card-Photos")
                        );
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error occurred while uploading the new file: " + ex.Message);
                    }
                }

                // Update the course and save changes
                await _unitOfWork.Courses.UpdateAsync(OldCourse);
                await _unitOfWork.Complete();

                return "Course Updates Successfully";
            }
            catch (Exception)
            {
                // Re-throw the exception with a custom message or log it
                throw;
            }

            //delete course
        }
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
                    Course.IsDeleted = true;
                    await _unitOfWork.Courses.UpdateAsync(Course);
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



        public async Task<SearchCoursesResopnseDto> Search(SearchCoursesDtos searchCoursesDtos, int PageNumber, int PageSize)
        {
            SearchCoursesResopnseDto searchCoursesResopnseDto = null;
            try
            {
                string jsonData = await _unitOfWork.ExecuteStoredProcedureAsStringAsync(
                    "SearchCourses",
                    "@CourseName", searchCoursesDtos.CourseName,
                    "@CourseDescription", searchCoursesDtos.CourseDescription,
                    "@CategoryId", searchCoursesDtos.CategoryId,
                    "@Level", searchCoursesDtos.Level,
                    "@Language", searchCoursesDtos.Language,
                    "@PageSize", PageSize,
                    "@PageNumber", PageNumber
                );

                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Deserialize JSON if needed
                     searchCoursesResopnseDto = JsonSerializer.Deserialize<SearchCoursesResopnseDto>(jsonData);

                    // Process the response
                    return searchCoursesResopnseDto;
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
  
            return searchCoursesResopnseDto;
        }
            
        public async Task<CourseDetailsResponseDto> GetCourseDetailsWithFeedbacksAsync(int courseId)
        {
            CourseDetailsResponseDto courseDetailsResponse = null;

            try
            {
                // Execute the stored procedure and retrieve JSON data
                var jsonData = await _unitOfWork.ExecuteStoredProcedureAsStringAsync(
                    "GetCourseDetailsWithFeedbacks",
                    "@CourseId", courseId
                );

                // Check if JSON data is valid
                if (!string.IsNullOrEmpty(jsonData))
                {
                    // Deserialize JSON into the response DTO
                    courseDetailsResponse = JsonSerializer.Deserialize<CourseDetailsResponseDto>(jsonData);
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

            return courseDetailsResponse;
        }

    }
}
