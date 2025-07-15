public interface IGenericService<TDto, TEntity> where TDto : class where TEntity : class
{
    Task<TDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> CreateAsync(TDto dto);
    Task<TDto> UpdateAsync(Guid id, TDto dto);
    Task<bool> DeleteAsync(Guid id);
}
