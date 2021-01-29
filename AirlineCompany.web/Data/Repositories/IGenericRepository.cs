using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Data.Repositories
{
    public interface IGenericRepository<T> where T: class
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(int ID);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int ID);
    }
}
