namespace Codixa.Core.Dtos.SectionsDtos.TestSection.request
{
    public class AddNewQuestionDto
    {
        public string Text { get; set; }
        public List<AddNewChoiceAnswerDto> ChoiceAnswer { get; set; }
    }
}

