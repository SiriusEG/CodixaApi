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
using Codixa.Core.Dtos.FeedbackDto;
using Codixa.Core.Enums;
using System.Data;
using Codxia.EF;

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
                    CategoryId = Course.CategoryId
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
        public async Task<(List<GetAllCoursesDetailsResponseDto> Courses,int PageCount)> GetUserCourses(string token,int PageNumber,int PageSize )
        {

            try
            {
                var UserID = await _authenticationService.GetUserIdFromToken(token);
                if (UserID != null) {
                    var(result, totalCount) = await _unitOfWork.ExecuteStoredProcedureWithCountAsync<GetAllCoursesDetailsResponseDto>("GetCoursesByUserId", 
                        new SqlParameter("@UserId", UserID),
                        new SqlParameter("@PageNumber", PageNumber),
                        new SqlParameter("@PageSize", PageSize));
                    int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
                    return (result, totalPages);
                }
                throw new UserNotFoundException("Login To Get Your Courses!");
            }
            catch (UserNotFoundException) {
                throw;
            }


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

        public async Task<(List<SearchCoursesResopnseDto> Courses, int PageCount)> Search(SearchCoursesDtos searchCoursesDtos, int PageNumber, int PageSize)
        {
         
            try
            {

                if (searchCoursesDtos != null)
                {
                    var (result, totalCount) = await _unitOfWork.ExecuteStoredProcedureWithCountAsync<SearchCoursesResopnseDto>("SearchCourses",
                        new SqlParameter("@CourseName", searchCoursesDtos.CourseName),
                        new SqlParameter("@CourseDescription", searchCoursesDtos.CourseDescription),
                        new SqlParameter("@CategoryId", searchCoursesDtos.CategoryId),
                        new SqlParameter("@PageSize", PageSize),
                        new SqlParameter("@PageNumber", PageNumber));
                   
                    return (result, totalCount);
                }
                throw new Exception("there are an error with Searching Filters");

            }
            catch (Exception ex)
            {
                throw new Exception("there are an error while Searching Courses " + ex.Message);
            }
          
        }

        public async Task<CourseDetailsResponseDto> GetCourseDetailsWithFeedbacksAsync(int courseId)
        {
            CourseDetailsResponseDto courseDetailsResponse = null;

            try
            {
                // Open the database connection
                using var connection = new SqlConnection(_Context.Database.GetConnectionString());
                await connection.OpenAsync();

                // Create and configure the SQL command
                using var command = connection.CreateCommand();
                command.CommandText = "GetCourseDetailsWithFeedbacks";
                command.CommandType = CommandType.StoredProcedure;

                // Add parameter for @CourseId
                command.Parameters.Add(new SqlParameter("@CourseId", courseId));

                // Execute the stored procedure and read the results
                using var reader = await command.ExecuteReaderAsync();

                // Initialize the response DTO
                courseDetailsResponse = null;

                // Read the first result set (course details)
                if (await reader.ReadAsync())
                {
                    courseDetailsResponse = new CourseDetailsResponseDto
                    {
                        CourseName = reader["CourseName"]?.ToString() ?? string.Empty,
                        CourseDescription = reader["CourseDescription"]?.ToString() ?? string.Empty,
                        CourseCardPhotoPath = reader["CourseCardPhotoPath"]?.ToString() ?? string.Empty,
                        CategoryName = reader["CategoryName"]?.ToString() ?? string.Empty,
                        InsrtuctorName = reader["InstructorName"]?.ToString() ?? string.Empty,
                        SectionCount = reader.IsDBNull(reader.GetOrdinal("SectionCount")) ? 0 : reader.GetInt32(reader.GetOrdinal("SectionCount")),
                        TotalRate = (RateEnum)(reader.IsDBNull(reader.GetOrdinal("TotalRate")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalRate"))),
                        Count5Stars = reader.IsDBNull(reader.GetOrdinal("Count5Stars")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count5Stars")),
                        Count4Stars = reader.IsDBNull(reader.GetOrdinal("Count4Stars")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count4Stars")),
                        Count3Stars = reader.IsDBNull(reader.GetOrdinal("Count3Stars")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count3Stars")),
                        Count2Stars = reader.IsDBNull(reader.GetOrdinal("Count2Stars")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count2Stars")),
                        Count1Star = reader.IsDBNull(reader.GetOrdinal("Count1Star")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count1Star")),
                        FeedBacks = new List<FeedBackCourseDto>() 
                    };
                }

                // Move to the second result set (feedbacks)
                if (await reader.NextResultAsync())
                {
                    // Read the second result set (feedbacks) and populate the FeedBacks list
                    while (await reader.ReadAsync())
                    {
                        courseDetailsResponse.FeedBacks.Add(new FeedBackCourseDto
                        {
                            Rate = (RateEnum)reader.GetInt32(reader.GetOrdinal("Rate")),
                            Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? string.Empty : reader.GetString(reader.GetOrdinal("Comment")),
                            StudentFullName = reader["StudentFullName"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (InvalidOperationException ioEx)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw; 
            }

            // Return the result (or null if an error occurred)
            return courseDetailsResponse;
        }

    }
}
