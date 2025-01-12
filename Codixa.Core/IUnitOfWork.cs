using Codixa.Core.Interfaces;
using Codixa.Core.Models.UserModels;
using Codxia.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Codxia.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Student> Students { get; }
        IBaseRepository<Instructor> Instructors { get; }
        IBaseRepository<InstructorJoinRequest> InstructorJoinRequests { get; }

        IUserRepository UsersManger { get; }
        IFileRepository Files { get; }
        Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params object[] parameters) where T : class;

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> Complete();
    }
}
