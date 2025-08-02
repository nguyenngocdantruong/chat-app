using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{

    public interface IGenericService<TEntity>
    where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
