using Delivery.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepositoryAsync<T> where T : class
    {
        private readonly DeliveryDbContext _db;
        private readonly DbSet<T> _set;

        public EfRepository(DeliveryDbContext db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _set.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<T> GetAsync(Guid id)
        {
            // Assume entity has Id property; use FindAsync if primary key known
            return await _set.FindAsync(id) as T ?? null!;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int page, int amount)
        {
            return await _set.Skip((page - 1) * amount).Take(amount).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await Task.CompletedTask;
        }
    }
}
