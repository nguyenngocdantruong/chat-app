namespace ChatApp.Application.Interfaces
{
    public interface IDtoMapper<TRequestDto, TEntity, TResponseDto>
        where TRequestDto : class
        where TEntity : class
        where TResponseDto : class
    {
        TEntity MapToEntity(TRequestDto requestDto);
        TResponseDto MapToResponseDto(TEntity entity);
    }
}
