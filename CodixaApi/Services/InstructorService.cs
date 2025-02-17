using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.InstructorDtos.Request;
using Codixa.Core.Enums;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codxia.Core;
using Codxia.EF;
using Microsoft.EntityFrameworkCore;

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

        public async Task<object> GetStudentRequestToEnrollCourse(string token, int courseId, int pageNumber, int pageSize)
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
                    var totalRecords = await _appDbContext.CourseRequests
                        .Where(cr => cr.CourseId == courseId && cr.Course.InstructorId == instructorId)
                        .AsNoTracking()
                        .CountAsync();

                    var query = _appDbContext.CourseRequests
                        .Where(cr => cr.CourseId == courseId && cr.Course.InstructorId == instructorId)
                        .OrderBy(cr => cr.RequestDate)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(cr => new
                        {
                            RequestId = cr.RequestId,
                            StudentName = cr.Student.StudentFullName,
                            CourseName = cr.Course.CourseName,
                            RequestStatus = cr.RequestStatus,
                            RequestDate = cr.RequestDate
                        })
                        .AsNoTracking(); // تحسين الأداء

                    return new
                    {
                        Data = await query.ToListAsync(),
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
                        await _UnitOfWork.Enrollments.AddAsync(new Enrollment
                        {
                            CourseId = Request.CourseId,
                            StudentId = Request.StudentId,
                            EnrollmentDate = DateTime.Now
                        });
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
