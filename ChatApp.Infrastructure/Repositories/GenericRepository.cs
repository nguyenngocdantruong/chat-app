using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ArgumentNullException = ChatApp.Domain.Exceptions.Runtime.ArgumentNullException;

namespace ChatApp.Infrastructure.Repositories
{

    public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
        where T : BaseEntity
    {
        protected readonly DbSet<T> DbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(m => m.Guid == id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => e.Guid == id);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentException("Entities collection cannot be null or empty.", nameof(entities));
            }
            DbSet.RemoveRange(entities);
        }

        public async Task<int> CountAsync(Predicate<T>? predicate = null)
        {
            if (predicate == null)
            {
                return await DbSet.CountAsync();
            }
            var query = DbSet.AsQueryable().Where(e => predicate(e));
            return await query.CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await DbSet.AddAsync(entity);
            return result.Entity;
        }

        public async Task<bool> Update(T entity)
        {
            try
            {
                var result = DbSet.Update(entity);
                return await Task.FromResult(true);
            }
            catch (Exception)
            {
                throw new DatabaseOperationException($"Can't update the entity {nameof(entity)}");
            }
        }

        public async Task<bool> Delete(T entity)
        {
            var result = DbSet.Remove(entity);
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
            return await Task.FromResult(DbSet.AsQueryable());
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"Entities collection cannot be null or empty. {nameof(entities)}");
            }
            await DbSet.AddRangeAsync(entities);
            return await Task.FromResult(entities);
        }
    }

}
