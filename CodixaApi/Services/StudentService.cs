using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codxia.Core;

namespace CodixaApi.Services
{
    public class StudentService: IStudentService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IAuthenticationService _AuthenticationService;

        public StudentService(IUnitOfWork unitOfWork , IAuthenticationService authenticationService)
        {
           _UnitOfWork = unitOfWork;
           _AuthenticationService = authenticationService;
        }

        //send Request enroll course
        public async Task<string> RequestToEnrollCourse(int CourseId,String token)
        {
            try
            {
                var UserId = await _AuthenticationService.GetUserIdFromToken(token);
                var Student = await _UnitOfWork.Students.FirstOrDefaultAsync(x=>x.UserId == UserId);
                if (UserId == null | Student==null)
                {
                    throw new UserNotFoundException("SignIn to enroll This course");
                }
                if (Student != null) {
                    var ReqExist = await _UnitOfWork.CourseRequests.FirstOrDefaultAsync(x => x.StudentId == Student.StudentId && x.CourseId == CourseId);
                    if (ReqExist != null) {
                        throw new Exception("Request Already Sent");
                    }

                }
                try
                {
                    await _UnitOfWork.CourseRequests.AddAsync(new CourseRequest
                    {
                        CourseId = CourseId,
                        StudentId = Student != null ? Student.StudentId : 0,
                        RequestDate = DateTime.Now
                    });
                    await _UnitOfWork.Complete();
                }
                catch (Exception ex) { 
                    throw new Exception("there are error while sending request" + ex);
                }
                   

                return ("Request Sent Successfully");
            }
            catch(UserNotFoundException) {
                throw;
            }
            catch (Exception) {
                throw;
            }
            
        } 


    }
}
