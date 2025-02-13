using Codxia.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected AppDbContext _Context;

        public BaseRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            _Context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }
       
        public async Task<T> GetByIdAsync(int id) => await _Context.Set<T>().FindAsync(id);

        public Task AddRangeAsync(IEnumerable<T> entities)
        {
            return _Context.Set<T>().AddRangeAsync(entities);
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _Context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task<T> UpdateAsync(T entity)
        {
            _Context.Set<T>().Update(entity);
            return Task.FromResult(entity);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> keySelector,
        params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            IQueryable<T> query = _Context.Set<T>();

            // Apply includes if they are provided
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            // Apply the filter and get the first result
            return await query.FirstOrDefaultAsync(keySelector);


        }


        public async Task<ICollection<T>> GetListOfEntitiesByIdIncludesAsync(
        Expression<Func<T, bool>> keySelector,
        params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            var query = _Context.Set<T>().AsQueryable();

            // Apply each include expression to the query
            foreach (var include in includes)
            {
                query = include(query);
            }

            return await query.Where(keySelector).ToListAsync();
        }



    }
}
