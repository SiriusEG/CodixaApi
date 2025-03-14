using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response
{
    public class ReturnAllInstructorsReqDto : IKeylessEntity
    {
        public List<InstructorsReqDto> InstructorsRequests { get; set; }
        public int Totalpages { get; set; }
    }
}
