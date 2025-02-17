using Codixa.Core.Enums;

namespace Codixa.Core.Dtos.InstructorDtos.Request
{
    public class ChangeStudentRequestDto
    {
        public int RequestId { get; set; }
        public RequestStatusEnum NewStatus { get; set; }
    }
}
