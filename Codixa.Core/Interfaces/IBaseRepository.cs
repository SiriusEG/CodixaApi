﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<ICollection<T>> GetListOfEntitiesByIdIncludesAsync(Expression<Func<T, bool>> keySelector,params Func<IQueryable<T>, IQueryable<T>>[] includes);
        Task<T> UpdateAsync(T entity);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> keySelector,
        params Func<IQueryable<T>, IQueryable<T>>[] includes);

    }
}
