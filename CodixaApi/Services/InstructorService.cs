using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.InstructorDtos.Request;
using Codixa.Core.Enums;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.StudentCourseModels;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Codxia.EF;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace CodixaApi.Services
{
    public class InstructorService: IInstructorService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IAuthenticationService _AuthenticationService;
        private readonly AppDbContext _appDbContext;

        public InstructorService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService,AppDbContext appDbContext)
        {
            _UnitOfWork = unitOfWork;
            _AuthenticationService = authenticationService;
            _appDbContext = appDbContext;
        }

        public async Task<object> GetStudentRequestToEnrollCourse(
        string token,
        int courseId,
        int pageNumber,
        int pageSize,
        string searchTerm = null) // إضافة متغير للبحث
        {
            try
            {
                var UserId = await _AuthenticationService.GetUserIdFromToken(token);
                if (UserId == null)
                {
                    throw new UserNotFoundException("Sign in to view join requests");
                }

                var instructorId = await _appDbContext.Instructors
                    .Where(x => x.UserId == UserId)
                    .Select(x => x.InstructorId)
                    .FirstOrDefaultAsync();

                if (instructorId != 0)
                {
                    var query = _appDbContext.CourseRequests
                        .Where(cr => cr.CourseId == courseId && cr.Course.InstructorId == instructorId);

                    // تطبيق البحث إذا كان هناك قيمة مدخلة
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query = query.Where(cr =>
                            cr.Student.StudentFullName.Contains(searchTerm) ||
                            cr.Student.User.UserName.Contains(searchTerm) ||
                            cr.Student.User.Email.Contains(searchTerm));
                    }

                    var totalRecords = await query.CountAsync();

                    var data = await query
                        .OrderBy(cr => cr.RequestDate)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(cr => new
                        {
                            RequestId = cr.RequestId,
                            StudentName = cr.Student.StudentFullName,
                            UserName = cr.Student.User.UserName,
                            Email = cr.Student.User.Email,
                            CourseName = cr.Course.CourseName,
                            RequestStatus = cr.RequestStatus,
                            RequestDate = cr.RequestDate
                        })
                        .AsNoTracking()
                        .ToListAsync();

                    return new
                    {
                        Data = data,
                        PageNumber = pageNumber,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                    };
                }

                throw new Exception("Sign in to view join requests");
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<object> ChangeStudentRequestStatus(string token, ChangeStudentRequestDto changeStudentRequestDto)
        {
            try
            {
                CourseRequest Request = await _UnitOfWork.CourseRequests.FirstOrDefaultAsync(x=>x.RequestId == changeStudentRequestDto.RequestId);
                var result="";

                var UserId = await _AuthenticationService.GetUserIdFromToken(token);
                if (UserId == null)
                {
                    throw new UserNotFoundException("Sign in to view join requests");
                }
                var instructorId = await _appDbContext.Instructors
               .Where(x => x.UserId == UserId)
               .Select(x => x.InstructorId)
               .FirstOrDefaultAsync();

                if (instructorId == 0)
                {
                    throw new UserNotFoundException("Sign in to view join requests");
                }

                switch (changeStudentRequestDto.NewStatus) {

                    case RequestStatusEnum.Rejected:
                        if(Request.RequestStatus == RequestStatusEnum.Accepted.ToString())
                        { 
                            var Enrollment = await _UnitOfWork.Enrollments.FirstOrDefaultAsync(x=>x.CourseId==Request.CourseId);
                            await _UnitOfWork.Enrollments.DeleteAsync(Enrollment);
                        }
                        Request.ReviewDate = DateTime.Now;
                        Request.ReviewedBy = instructorId;  
                        Request.RequestStatus = RequestStatusEnum.Rejected.ToString();
                        await _UnitOfWork.CourseRequests.UpdateAsync(Request);
                        await _UnitOfWork.Complete();
                        result = "Student Rejected";
                        break;

                    case RequestStatusEnum.Accepted:
                        Request.ReviewDate = DateTime.Now;
                        Request.ReviewedBy = instructorId;
                        Request.RequestStatus = RequestStatusEnum.Accepted.ToString();
                        await _UnitOfWork.CourseRequests.UpdateAsync(Request);

                        Enrollment enrollment = await _UnitOfWork.Enrollments.FirstOrDefaultAsync(x => x.CourseId == Request.CourseId && x.StudentId == Request.StudentId);
                       
                        if(enrollment == null)
                        {
                            Enrollment enrollment1 = new Enrollment
                            {
                                CourseId = Request.CourseId,
                                StudentId = Request.StudentId,
                                EnrollmentDate = DateTime.Now
                            };
                     

                            await _UnitOfWork.Enrollments.AddAsync(enrollment1);
                        }


                        CourseProgress courseProgress = await _UnitOfWork.CourseProgress.FirstOrDefaultAsync(x => x.CourseId == Request.CourseId && x.StudentId == Request.StudentId);
                   
                        if (courseProgress == null)
                        {
                            CourseProgress courseProgress1 = new CourseProgress
                            {
                                CourseId = Request.CourseId,
                                StudentId = Request.StudentId,
                                ProgressPercentage = 0.0
                            };
                      

                           await  _UnitOfWork.CourseProgress.AddAsync(courseProgress1);
                        }

                        var section = await _UnitOfWork.Sections.FirstOrDefaultAsync(x => x.SectionOrder == 1 && x.CourseId == Request.CourseId);
                        var lesson = section != null
                            ? await _UnitOfWork.Lessons.FirstOrDefaultAsync(x => x.LessonOrder == 1 && x.SectionId == section.SectionId)
                            : null;


                       
                        if (lesson != null && section != null)
                        {
                            LessonProgress lessonProgress = await _UnitOfWork.LessonProgress.FirstOrDefaultAsync(x => x.LessonId == lesson.LessonId && x.StudentId == Request.StudentId && x.SectionId == section.SectionId);
                            if (lessonProgress == null)
                            {
                                LessonProgress lessonProgress1 = new LessonProgress
                                {
                                    StudentId = Request.StudentId,
                                    LessonId = lesson.LessonId,
                                    SectionId = section.SectionId
                                };
                
                                await _UnitOfWork.LessonProgress.AddAsync(lessonProgress1);
                            }
                        }

                       
                        if (section != null) {
                            SectionProgress sectionProgress = await _UnitOfWork.SectionProgress.FirstOrDefaultAsync(x => x.StudentId == Request.StudentId && x.SectionId == section.SectionId && x.CourseId == Request.CourseId);
                           
                            if (sectionProgress == null)
                            {
                                SectionProgress sectionProgress1 = new SectionProgress
                                {
                                    StudentId = Request.StudentId,
                                    SectionId = section.SectionId,
                                    CourseId = Request.CourseId
                                };
                      
                                await _UnitOfWork.SectionProgress.AddAsync(sectionProgress1);
                            }                        
                        }

                        await _UnitOfWork.Complete();
                        result = "Student Accepted";
                        break;

                }

                return result;


            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (Exception) {
                throw;
            }

        }


    }
}
