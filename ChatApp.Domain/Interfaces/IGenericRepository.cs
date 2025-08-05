using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
        public interface IGenericRepository<T> where T : BaseEntity
        {
            Task<IQueryable<T>> GetAllAsync();
            Task<T?> GetByIdAsync(Guid id);
            Task<T> AddAsync(T entity);
            Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
            Task<bool> ExistsAsync(Guid id);
            Task<bool> Update(T entity);
            Task<bool> Delete(T entity);
            Task<int> DeleteRange(IEnumerable<T> entities);  
            Task<int> CountAsync(Predicate<T>? predicate = null);
    }
}
