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
using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Models.CourseModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

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

        public UnitOfWork(AppDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _Context = context;
            _environment = environment;
            _userManager = userManager;
            UsersManger = new UserRepository(_userManager, context);
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
                    return await _Context.Database.ExecuteSqlRawAsync(storedProcedure, parameters);
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

        public async Task<(List<T> Results, int TotalCount)> ExecuteStoredProcedureWithCountAsync<T>(
    string storedProcedure, params object[] parameters) where T : class, new()
        {
            using var connection = new SqlConnection(_Context.Database.GetConnectionString()); // Open connection
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            foreach (var param in parameters)
            {
                if (param is SqlParameter sqlParam)
                    command.Parameters.Add(sqlParam);
            }

            using var reader = await command.ExecuteReaderAsync(); // Execute the stored procedure

            // Read the total count (first result set)
            int totalCount = 0;
            if (await reader.ReadAsync())
            {
                totalCount = reader.GetInt32(0); // ✅ Read total count from first row
            }

            // ✅ Move to the second result set (paginated courses)
            if (await reader.NextResultAsync())
            {
                var results = new List<T>();
                var properties = typeof(T).GetProperties();

                while (await reader.ReadAsync())
                {
                    var obj = Activator.CreateInstance<T>();

                    foreach (var prop in properties)
                    {
                        var value = reader[prop.Name];
                        if (value != DBNull.Value)
                        {
                            prop.SetValue(obj, value);
                        }
                    }
                    results.Add(obj);
                }

                return (results, totalCount); // ✅ Return both paginated courses & total count
            }
            return (new List<T>(), totalCount);
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
