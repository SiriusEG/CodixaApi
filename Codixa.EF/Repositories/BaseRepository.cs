using Codxia.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public T Add(T entity)
        {
            _Context.Set<T>().Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll() => _Context.Set<T>().ToList();
        public async Task<IEnumerable<T>> GetAllAsync() => await _Context.Set<T>().ToListAsync();
        public T GetById(int id) => _Context.Set<T>().Find(id);
        public void AddRange(IEnumerable<T> entities)
        {
            _Context.Set<T>().AddRange(entities);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _Context.Set<T>().RemoveRange(entities);
        }
        public T Update(T entity)
        {
            _Context.Set<T>().Update(entity);
            return entity;
        }
    }
}
