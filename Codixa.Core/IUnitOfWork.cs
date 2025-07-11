using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.sharedModels;
using Codixa.Core.Models.StudentCourseModels;
using Codixa.Core.Models.UserModels;
using Codxia.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace Codxia.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Student> Students { get; }
        IBaseRepository<Instructor> Instructors { get; }
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Lesson> Lessons { get; }
        IBaseRepository<Section> Sections { get; }
        IBaseRepository<Enrollment> Enrollments { get; }
        IBaseRepository<Course> Courses { get; }
        IBaseRepository<CourseRequest> CourseRequests { get; }
        IBaseRepository<RefreshToken> RefreshTokens { get; }
        IBaseRepository<InstructorJoinRequest> InstructorJoinRequests { get; }
        IBaseRepository<courseFeedback> courseFeedbacks { get; }
        IBaseRepository<ChoicesQuestion> ChoicesQuestions { get; }
        IBaseRepository<UserAnswer> UserAnswers { get; }
        IBaseRepository<TestResult> TestResults { get; }
        IBaseRepository<SectionTest> SectionTests { get; }
        IBaseRepository<Question> Questions { get; }
        IBaseRepository<SectionProgress> SectionProgress { get; }
        IBaseRepository<LessonProgress> LessonProgress { get; }
        IBaseRepository<CourseProgress> CourseProgress { get; }
        IBaseRepository<StudentTestAttempt> StudentTestAttempts { get; }
        IBaseRepository<Certification> Certifications { get; }

        IUserRepository UsersManger { get; }
        IFileRepository Files { get; }
        Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params object[] parameters) where T : class;
        Task<int> ExecuteStoredProcedureAsyncIntReturn(string storedProcedure, params object[] parameters);
        Task<int> ExecuteStoredProcedureAsyncIntReturnScalar(string storedProcedure, params SqlParameter[] parameters);
        Task<string> ExecuteStoredProcedureAsStringAsync(string storedProcedure, params object[] parameters);

        Task<List<T>> ExecuteTableValuedFunctionAsync<T>(string tvfName, params object[] parameters) where T : class;

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> Complete();
    }
}
