using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{

    public interface IGenericService<in TEntity,TResponseDto>
    where TEntity : BaseEntity
    where TResponseDto : class
    {
        Task<TResponseDto?> GetByIdAsync(Guid id);
        Task<TResponseDto> CreateAsync(TEntity entity);
        Task UpdateAsync(Guid id, TEntity entity);
        Task DeleteAsync(Guid id);
    }
}
