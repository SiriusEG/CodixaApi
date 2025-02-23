using Codixa.Core.Dtos.FeedbackDto;

namespace Codixa.Core.Interfaces
{
    public interface ICourseFeedbackService
    {
        Task<string> Delete(int CourseId, string token);
        Task<string> Update(FeedBackDto newbackRequestDto, int CourseId, string token);
        Task<string> AddFeedback(FeedBackDto backRequestDto, int CourseId, string token);
    }
}
