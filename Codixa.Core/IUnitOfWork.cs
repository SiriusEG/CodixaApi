﻿using Codixa.Core.Interfaces;
using Codixa.Core.Models.CourseModels;
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

        IUserRepository UsersManger { get; }
        IFileRepository Files { get; }
        Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params object[] parameters) where T : class;
        Task<int> ExecuteStoredProcedureAsyncIntReturn(string storedProcedure, params object[] parameters);
        Task<int> ExecuteStoredProcedureAsyncIntReturnScalar(string storedProcedure, params SqlParameter[] parameters);

        Task<List<T>> ExecuteTableValuedFunctionAsync<T>(string tvfName, params object[] parameters) where T : class;

        Task<(List<T> Results, int TotalCount)> ExecuteStoredProcedureWithCountAsync<T>(
        string storedProcedure, params object[] parameters) where T : class, new();

    Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> Complete();
    }
}
