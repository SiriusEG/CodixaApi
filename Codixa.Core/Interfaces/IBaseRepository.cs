using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codxia.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);

        IEnumerable<T> GetAll();
        T Add(T entity);
        T Update(T entity);

        void Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        void DeleteRange(IEnumerable<T> entities);
        void AddRange(IEnumerable<T> entities);
    }
}
