using Codixa.Core.Enums;
using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request
{
    public class ChangeInstructorRequestStatusDto : IKeylessEntity
    {
        public int RequestId { get; set; }
        
        public RequestStatusEnum NewStatus { get; set; }
    }
}
