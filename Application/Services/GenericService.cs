using ChatApp.Application.Interfaces;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<T?> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);

        public async Task AddAsync(T entity)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Delete(entity);
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
        }

        public Task<T?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }
    }

}
