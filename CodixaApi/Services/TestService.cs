using Codixa.Core.Dtos.QuestionsDtos;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.SectionsTestsModels;
using Codxia.Core;
using Codxia.EF;
using Microsoft.EntityFrameworkCore;


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

        public async Task<string> AddAsnwers(List<QestionsAnswerDto> qestionsAnswerDto, string token)
        {
            try
            {
                var userId = await _authenticationService.GetUserIdFromToken(token);
                var student = await _UnitOfWork.Students.FirstOrDefaultAsync(x => x.UserId == userId);
                if (student == null)
                {
                    throw new Exception("Unauthorized or student not found.");
                }

                // Load question with its SectionTest using the include method
                var question = await _UnitOfWork.Questions.FirstOrDefaultAsync(
                    q => q.QuestionId == qestionsAnswerDto[0].QuestionId,
                    q => q.Include(x => x.SectionTest)
                );

                if (question == null || question.SectionTest == null)
                {
                    throw new Exception("Invalid question or missing section test.");
                }

                var sectionTest = question.SectionTest;

                // ✅ Check if student already passed this test or scored 100
                var testResults = await _UnitOfWork.TestResults.GetEntitesListBy(
                    r => r.StudentId == student.StudentId && r.SectionTestId == sectionTest.SectionTestId
                );

                var alreadyPassed = testResults.Any(r => r.IsPassed || r.Result == 100);
                if (alreadyPassed)
                {
                    throw new Exception("You have already passed this test and cannot retake it.");
                }

                // Count existing attempts using GetEntitesListBy
                var existingAttempts = await _UnitOfWork.StudentTestAttempts.GetEntitesListBy(
                    a => a.StudentId == student.StudentId && a.SectionTestId == sectionTest.SectionTestId
                );

                if (existingAttempts.Count >= sectionTest.MaxAttempts)
                {
                    throw new Exception("You have exceeded the maximum number of attempts.");
                }

                // Create and save new attempt
                var newAttempt = new StudentTestAttempt
                {
                    StudentId = student.StudentId,
                    SectionTestId = sectionTest.SectionTestId,
                    AttemptDate = DateTime.UtcNow,
                    AttemptNumber = existingAttempts.Count + 1
                };

                await _UnitOfWork.StudentTestAttempts.AddAsync(newAttempt);
                await _UnitOfWork.Complete();

                // Prepare and save answers
                var userAnswers = qestionsAnswerDto.Select(dto => new UserAnswer
                {
                    AttemptId = newAttempt.AttemptId,
                    QuestionId = dto.QuestionId,
                    StudentId = student.StudentId,
                    SelectedChoicesQuestionId = dto.SelectedChoicesQuestionId
                }).ToList();

                await _UnitOfWork.UserAnswers.AddRangeAsync(userAnswers);
                await _UnitOfWork.Complete();

                // Call stored procedure with the new attempt ID
                var jsonData = await _UnitOfWork.ExecuteStoredProcedureAsStringAsync(
                    "EvaluateStudentTest",
                    "@StudentId", student.StudentId,
                    "@SectionTestId", sectionTest.SectionTestId,
                    "@AttemptId", newAttempt.AttemptId
                );

                return jsonData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error submitting answers: " + ex.Message);
            }
        }




    }
}
