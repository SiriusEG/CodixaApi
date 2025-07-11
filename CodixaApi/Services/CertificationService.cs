using Codixa.Core.Dtos.CertificationDtos;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.UserModels;
using Codxia.Core;
using Microsoft.EntityFrameworkCore;
namespace CodixaApi.Services
{
    public class CertificationService:ICertificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public CertificationService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }


        //verify Certification

        public async Task<GetCertDto> VerifyCertification(string CertId)
        {
            try
            {
                var Certification = await _unitOfWork.Certifications.FirstOrDefaultAsync(x => x.CertificationId==CertId, x => x.Include(x => x.Course), x => x.Include(x => x.Student), x => x.Include(x => x.Instructor));

                if (Certification == null) {
                    throw new Exception("There is no certificate with this number");
                }

                return new GetCertDto
                {
                    CertificationId = Certification.CertificationId,
                    StudntName = Certification.Student.StudentFullName,
                    InstructorName = Certification.Instructor.InstructorFullName,
                    CourseName = Certification.Course.CourseName,
                    CertificationDate = Certification.CertificationIssueDate.ToString("MMMM dd, yyyy")
                };
            }

            catch (Exception ex)
            {
                throw new Exception("there are error while Getting CertData " + ex.Message);
            }
        }

        //GetCertificationInfoByStudentCourse
        //GetCertDto

        public async Task<GetCertDto> GetCertification(int CourseId,string token)
        {
            try
            {
                var UserID = await _authenticationService.GetUserIdFromToken(token);
                if (UserID == null)
                {
                    throw new Exception("Missing Authorization");
                }
                var student = await _unitOfWork.Students.FirstOrDefaultAsync(x => x.UserId == UserID);

                var Certification = await _unitOfWork.Certifications.FirstOrDefaultAsync(x=>x.StudentId == student.StudentId && x.CourseId == CourseId, x=>x.Include(x=>x.Course), x => x.Include(x => x.Student), x => x.Include(x => x.Instructor));

                return new GetCertDto
                {
                    CertificationId = Certification.CertificationId,
                    StudntName = Certification.Student.StudentFullName,
                    InstructorName = Certification.Instructor.InstructorFullName,
                    CourseName = Certification.Course.CourseName,
                    CertificationDate = Certification.CertificationIssueDate.ToString("MMMM dd, yyyy")
                };
            }

            catch (Exception ex)
            {
                throw new Exception("there are error while Getting CertData " + ex.Message);
            }


        }

    }
}
