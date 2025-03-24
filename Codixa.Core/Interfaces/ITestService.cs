using Codixa.Core.Dtos.QuestionsDtos;

namespace Codixa.Core.Interfaces
{
    public interface ITestService
    {
        Task<string> AddAsnwers(List<QestionsAnswerDto> qestionsAnswerDto, string token);
    }
}
