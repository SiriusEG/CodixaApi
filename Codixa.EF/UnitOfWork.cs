using Codxia.Core.Interfaces;
using Codxia.EF.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Codxia.Core;
using Codixa.Core.Models.UserModels;
using Codixa.Core.Interfaces;
using Codixa.EF.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Models.CourseModels;
using Microsoft.Data.SqlClient;
using System.Data;
using Codixa.Core.Models.SectionsTestsModels;
using Codixa.Core.Models.StudentCourseModels;
using Codixa.Core.Models.sharedModels;

namespace Codxia.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _Context;
        private readonly UserManager<AppUser> _userManager;
        public IUserRepository UsersManger { get; private set; }
        private readonly IWebHostEnvironment _environment;
        public IBaseRepository<Category> Categories { get; private set; }
        public IBaseRepository<RefreshToken> RefreshTokens { get; private set; }
        public IBaseRepository<Student> Students { get; private set; }
        public IBaseRepository<Lesson> Lessons { get; private set; }
        public IBaseRepository<Section> Sections { get; private set; }
        public IBaseRepository<Instructor> Instructors { get; private set; }
        public IBaseRepository<InstructorJoinRequest> InstructorJoinRequests { get; private set; }
        public IFileRepository Files { get; private set; }
        public IBaseRepository<Course> Courses { get; private set; }
        public IBaseRepository<CourseRequest> CourseRequests { get; private set; }
        public IBaseRepository<Enrollment> Enrollments { get; private set; }
        public IBaseRepository<courseFeedback> courseFeedbacks { get; private set; }
        public IBaseRepository<ChoicesQuestion> ChoicesQuestions { get; private set; }
        public IBaseRepository<UserAnswer> UserAnswers { get; private set; }
        public IBaseRepository<TestResult> TestResults { get; private set; }
        public IBaseRepository<SectionTest> SectionTests { get; private set; }
        public IBaseRepository<Question> Questions { get; private set; }
        public IBaseRepository<SectionProgress> SectionProgress { get; private set; }
        public IBaseRepository<LessonProgress> LessonProgress { get; private set; }
        public IBaseRepository<CourseProgress> CourseProgress { get; private set; }
        public IBaseRepository<StudentTestAttempt> StudentTestAttempts { get; private set; }
        public IBaseRepository<Certification> Certifications { get; private set; }

        public UnitOfWork(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _Context = context;
            _environment = environment;
            _userManager = userManager;
            UsersManger = new UserRepository(_userManager, _Context);
            Students = new BaseRepository<Student>(_Context);
            Instructors = new BaseRepository<Instructor>(_Context);
            Categories = new BaseRepository<Category>(_Context);
            RefreshTokens = new BaseRepository<RefreshToken>(_Context);
            InstructorJoinRequests = new BaseRepository<InstructorJoinRequest>(_Context);
            Files = new FileRepository(_Context, _environment);

            Courses = new BaseRepository<Course>(_Context);
            Sections = new BaseRepository<Section>(_Context);
            Lessons = new BaseRepository<Lesson>(_Context);
            CourseRequests = new BaseRepository<CourseRequest>(_Context);
            Enrollments = new BaseRepository<Enrollment>(_Context);
            courseFeedbacks = new BaseRepository<courseFeedback>(_Context);
            ChoicesQuestions = new BaseRepository<ChoicesQuestion>(_Context);
            UserAnswers = new BaseRepository<UserAnswer>(_Context);
            TestResults = new BaseRepository<TestResult>(_Context);
            SectionTests = new BaseRepository<SectionTest>(_Context);
            Questions = new BaseRepository<Question>(_Context);
            SectionProgress = new BaseRepository<SectionProgress>(_Context);
            LessonProgress = new BaseRepository<LessonProgress>(_Context);
            CourseProgress = new BaseRepository<CourseProgress>(_Context);
            StudentTestAttempts = new BaseRepository<StudentTestAttempt>(_Context);
            Certifications = new BaseRepository<Certification>(_Context);

        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params object[] parameters) where T : class
        {

            if (parameters == null || parameters.Length == 0)
            {

                return await _Context.Set<T>().FromSqlRaw(storedProcedure).ToListAsync();
            }
            else
            {

                return await _Context.Set<T>().FromSqlRaw(storedProcedure, parameters).ToListAsync();
            }
        }


        public async Task<int> ExecuteStoredProcedureAsyncIntReturn(string storedProcedure, params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    return await _Context.Database.ExecuteSqlRawAsync(storedProcedure);
                }
                else
                {
                    var RowsEfficted =  await _Context.Database.ExecuteSqlRawAsync(storedProcedure, parameters);
                    return RowsEfficted;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message == "RequestId not found in InstructorJoinRequests.")
                {
                    throw new RequestIdnotFoundInInstructorJoinRequestsException("RequestId not found!");
                }

              
                return -1;
            }
        }

        public async Task<string> ExecuteStoredProcedureAsStringAsync(string storedProcedure, params object[] parameters)
        {
            try
            {
                if (parameters.Length % 2 != 0)
                {
                    throw new ArgumentException("Parameters must be provided in key-value pairs.");
                }

                using var connection = new SqlConnection(_Context.Database.GetConnectionString());
                await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = storedProcedure;
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters dynamically
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    string paramName = parameters[i]?.ToString();
                    object paramValue = parameters[i + 1];

                    if (string.IsNullOrEmpty(paramName))
                    {
                        throw new ArgumentException("Parameter name cannot be null or empty.");
                    }

                    var sqlParam = new SqlParameter(paramName, paramValue ?? (object)DBNull.Value);
                    command.Parameters.Add(sqlParam);
                }

                // Execute the stored procedure and read the result
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    string jsonData = reader[0]?.ToString();

                    if (string.IsNullOrWhiteSpace(jsonData))
                    {
                        throw new InvalidOperationException("The returned JSON is null or empty.");
                    }

                    return jsonData; // Return raw JSON string
                }

                return null; // No data returned
            }
            catch (SqlException sqlEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> ExecuteStoredProcedureAsyncIntReturnScalar(string storedProcedure, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = _Context.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = storedProcedure;
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                // Get the first column value (which should be TotalUpdates)
                                return reader.GetInt32(0);
                            }
                        }
                    }
                }

                return 0; // In case no rows were returned
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<List<T>> ExecuteTableValuedFunctionAsync<T>(string tvfName, params object[] parameters) where T : class
        {
            // بناء استعلام SELECT مع اسم الوظيفة والبارامترات
            var sqlQuery = $"SELECT * FROM {tvfName}({string.Join(", ", Enumerable.Range(0, parameters.Length).Select(i => "@" + i))})";

            // تحويل البارامترات إلى شكل يقبله FromSqlRaw
            var parameterObjects = parameters.Select((p, i) => new SqlParameter($"@{i}", p)).ToArray();

            return await _Context.Set<T>()
                                 .FromSqlRaw(sqlQuery, parameterObjects)
                                 .ToListAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _Context.Database.BeginTransactionAsync();
        }

        public async Task<int> Complete()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context.Dispose();
        }
    }
}
