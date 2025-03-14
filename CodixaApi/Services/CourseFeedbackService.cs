using Codixa.Core.Dtos.FeedbackDto;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.UserModels;
using Codxia.Core;

namespace CodixaApi.Services
{
    public class CourseFeedbackService : ICourseFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public CourseFeedbackService(IUnitOfWork unitOfWork,IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        //addfeedback
        public async Task<string> AddFeedback (FeedBackDto backRequestDto,int CourseId,string token)
        {
            try
            {

                string UserId = await _authenticationService.GetUserIdFromToken(token);
                Student Student = await _unitOfWork.Students.FirstOrDefaultAsync(x=>x.UserId == UserId);
                var FeedBack = await _unitOfWork.courseFeedbacks.FirstOrDefaultAsync(x => x.StudentId == Student.StudentId && x.CourseId == CourseId);
                if (FeedBack != null)
                {
                    throw new Exception("You Have Added FeedBack");
                }
                await _unitOfWork.courseFeedbacks.AddAsync(new courseFeedback
                {
                    Comment = backRequestDto.Comment,
                    CourseId = CourseId,
                    StudentId = Student.StudentId,
                    rate = backRequestDto.Rate,
                    CreatedAt = DateTime.Now
                });
                await _unitOfWork.Complete();

                return "FeedBack Added Successfully";
            }
            catch (Exception) {
                throw;
            }
        }
        //deletefeedback
        public async Task<string> Delete(int CourseId, string token)
        {
            try
            {

                string UserId = await _authenticationService.GetUserIdFromToken(token);
                Student Student = await _unitOfWork.Students.FirstOrDefaultAsync(x => x.UserId == UserId);
                var Feedback = await _unitOfWork.courseFeedbacks.FirstOrDefaultAsync(x=>x.StudentId == Student.StudentId && x.CourseId == CourseId);
                if (Feedback != null)
                {
                    await _unitOfWork.courseFeedbacks.DeleteAsync(Feedback);
                    await _unitOfWork.Complete();
                    return "FeedBack Deleted Successfully";
                }
                throw new Exception("FeedBack Delete failed");
            }
            catch (Exception)
            {
                throw;
            }
        }
        //updatefeedback
        public async Task<string> Update(FeedBackDto newbackRequestDto, int CourseId, string token)
        {
            try
            {

                string UserId = await _authenticationService.GetUserIdFromToken(token);
                Student Student = await _unitOfWork.Students.FirstOrDefaultAsync(x => x.UserId == UserId);
                var Feedback = await _unitOfWork.courseFeedbacks.FirstOrDefaultAsync(x => x.StudentId == Student.StudentId && x.CourseId == CourseId);
                if (Feedback != null)
                {
                    Feedback.rate =newbackRequestDto.Rate;
                    Feedback.Comment = newbackRequestDto.Comment;
                    await _unitOfWork.courseFeedbacks.UpdateAsync(Feedback);
                    await _unitOfWork.Complete();
                    return "FeedBack updated Successfully";
                }
                throw new Exception("FeedBack updated failed");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
