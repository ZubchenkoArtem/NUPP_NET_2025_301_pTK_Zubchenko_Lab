using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.Infrastructure.Repositories
{
    public interface IRepositoryAsync<T> where T : class
    {
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetPagedAsync(int page, int amount);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
