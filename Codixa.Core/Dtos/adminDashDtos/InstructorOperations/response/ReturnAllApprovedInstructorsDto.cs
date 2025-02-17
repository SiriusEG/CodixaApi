using Codixa.Core.Interfaces;

namespace Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response
{
    public class ReturnAllApprovedInstructorsDto : IKeylessEntity
    {
        public int InstructorId { get; set; }
        public string UserName { get; set; }
        public string InstructorFullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialty { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string FilePath { get; set; }

    }
}
