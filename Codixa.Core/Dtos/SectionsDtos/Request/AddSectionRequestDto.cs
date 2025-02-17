namespace Codixa.Core.Dtos.SectionsDtos.Request
{
    public class AddSectionRequestDto
    {
        public string SectionName { get; set; }

        public int SectionOrder { get; set; }

        public int CourseId { get; set; }

    }
}
