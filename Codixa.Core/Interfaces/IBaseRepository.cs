using System.Linq.Expressions;

namespace Codxia.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);

        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<T> GetQueryable();
        Task<T> GetByIdAsync(int id);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<ICollection<T>> GetListOfEntitiesByIdIncludesAsync(Expression<Func<T, bool>> keySelector,params Func<IQueryable<T>, IQueryable<T>>[] includes);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> keySelector,
        params Func<IQueryable<T>, IQueryable<T>>[] includes);
        Task<List<T>> GetEntitesListBy(Expression<Func<T, bool>> keySelector,
        params Func<IQueryable<T>, IQueryable<T>>[] includes);

    }
}
