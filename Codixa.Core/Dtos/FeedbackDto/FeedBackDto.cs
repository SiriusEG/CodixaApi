using Codixa.Core.Enums;

namespace Codixa.Core.Dtos.FeedbackDto
{
    public class FeedBackDto
    {
        public string? Comment { get; set; }
        public RateEnum Rate { get; set; }
    }
}
