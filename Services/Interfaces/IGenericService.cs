namespace ChatApp.Services.Interfaces
{

        public interface IGenericService<T> where T : class
        {
            Task<IEnumerable<T>> GetAllAsync();
            Task<T?> GetByIdAsync(object id);
            Task AddAsync(T entity);
            Task UpdateAsync(T entity);
            Task DeleteAsync(object id);
        }
    
}
