using Codixa.Core.Enums;

namespace Codixa.Core.Dtos.SectionsDtos.Request
{
    public class AddSectionRequestDto
    {
        public string SectionName { get; set; }

        public int SectionOrder { get; set; }

        public int CourseId { get; set; }

        public SectionTypeEnum SectionType { get; set; }
    }
}
