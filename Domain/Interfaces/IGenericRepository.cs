namespace ChatApp.Domain.Interfaces
{
        public interface IGenericRepository<T> where T : class
        {
            Task<IEnumerable<T>> GetAllAsync();
            Task<T?> GetByIdAsync(Guid id);
            Task AddAsync(T entity);
            Task AddRangeAsync(IEnumerable<T> entities);
            Task<bool> ExistsAsync(Guid id);
            void Update(T entity);
            void Delete(T entity);
            void DeleteRange(IEnumerable<T> entities);  
            Task<int> CountAsync(Predicate<T>? predicate = null);
    }
}
