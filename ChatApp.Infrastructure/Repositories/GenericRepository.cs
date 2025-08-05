using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _dbSet = context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Guid == id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Guid == id);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("Entities collection cannot be null or empty.", nameof(entities));
            }
            _dbSet.RemoveRange(entities);
        }

        public async Task<int> CountAsync(Predicate<T>? predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            var query = _dbSet.AsQueryable().Where(e => predicate(e));
            return await query.CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _dbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                var result = _dbSet.Update(entity);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                throw new DatabaseOperationException($"Can't update the entity {nameof(entity)}");
            }
        }

        public async Task<bool> Delete(T entity)
        {
            var result = _dbSet.Remove(entity);
            if (result == null)
            {
                throw new DatabaseOperationException($"Can't delete the entity {nameof(entity)}");
            }
            return await Task.FromResult(true);
        }

        Task<int> IGenericRepository<T>.DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.FromResult(_dbSet.AsQueryable());
        }

        async Task<IEnumerable<T>> IGenericRepository<T>.AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return await Task.FromResult(entities);
        }
    }

}
