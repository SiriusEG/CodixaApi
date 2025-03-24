using Codixa.Core.Dtos.QuestionsDtos;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.SectionsTestsModels;
using Codxia.Core;
using Codxia.EF;


namespace CodixaApi.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IAuthenticationService _authenticationService;

        public TestService(IUnitOfWork unitOfWork, IAuthenticationService authenticationService)
        {
            _UnitOfWork = unitOfWork;
            _authenticationService = authenticationService;
        }

        public async Task<string> AddAsnwers(List<QestionsAnswerDto> qestionsAnswerDto,string token)
        {
            try
            {
                var UserId = await _authenticationService.GetUserIdFromToken(token);
                var student = await _UnitOfWork.Students.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (student == null) {
                    throw new Exception("Missing Authorization");
                }


                List<UserAnswer> userAnswers = new List<UserAnswer>();

                foreach (var answer in qestionsAnswerDto)
                {
                    var oldanswer = await _UnitOfWork.UserAnswers.FirstOrDefaultAsync(x => x.StudentId == student.StudentId && x.QuestionId == answer.QuestionId);
                    if (oldanswer != null) {

                        throw new Exception("you have entered That Answers!");

                    }
                    userAnswers.Add(new UserAnswer
                    {
                        StudentId = student.StudentId,
                        QuestionId = answer.QuestionId,
                        SelectedChoicesQuestionId = answer.SelectedChoicesQuestionId
                    });
                }

                await _UnitOfWork.UserAnswers.AddRangeAsync(userAnswers);
                await _UnitOfWork.Complete();
                var SectionId = await _UnitOfWork.Questions.FirstOrDefaultAsync(x => x.QuestionId == qestionsAnswerDto[0].QuestionId);
                var jsonData = await _UnitOfWork.ExecuteStoredProcedureAsStringAsync(
                 "EvaluateStudentTest",
                 "@StudentId", student.StudentId,
                 "@SectionTestId", SectionId.SectionTestId

                );
                return jsonData;
            }
            catch (Exception ex) {
                throw;
            }
        }

    }
}
