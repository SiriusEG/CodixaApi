using Codxia.Core.Interfaces;
using Codxia.EF.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codxia.Core;
using Codixa.Core.Models.UserModels;
using Codixa.Core.Interfaces;
using Codixa.EF.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Codxia.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _Context;
        private readonly UserManager<AppUser> _userManager;
        public  IUserRepository UsersManger { get; private set; }
        private readonly IWebHostEnvironment _environment;
        public IBaseRepository<Student> Students { get; private set; }
        public IBaseRepository<Instructor> Instructors { get; private set; }
        public IBaseRepository<InstructorJoinRequest> InstructorJoinRequests { get; private set; }
        public IFileRepository Files { get; private set; }

        public UnitOfWork(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _Context = context;
            _environment = environment;
            _userManager = userManager;
            UsersManger = new UserRepository(_userManager);
            Students = new BaseRepository<Student>(_Context);
            Instructors = new BaseRepository<Instructor>(_Context);
            InstructorJoinRequests = new BaseRepository<InstructorJoinRequest>(_Context);
            Files = new FileRepository(_Context, _environment);


        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure, params object[] parameters) where T : class
        {
           
            return await _Context.Set<T>().FromSqlRaw(storedProcedure, parameters).ToListAsync();
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
