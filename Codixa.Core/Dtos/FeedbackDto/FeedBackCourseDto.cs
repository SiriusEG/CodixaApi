using Codixa.Core.Enums;

namespace Codixa.Core.Dtos.FeedbackDto
{
    public class FeedBackCourseDto
    {
        public string StudentFullName { get; set; }
        public string? Comment { get; set; }
        public RateEnum Rate { get; set; }
    }
}
