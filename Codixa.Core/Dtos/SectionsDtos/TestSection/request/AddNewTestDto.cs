namespace Codixa.Core.Dtos.SectionsDtos.TestSection.request
{
    public class AddNewTestDto
    {
        public int SectionId { get; set; }
        public decimal SuccessResult { get; set; }

        public List<AddNewQuestionDto> Questions { get; set; }
    }
}
