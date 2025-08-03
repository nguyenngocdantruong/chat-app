using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Services
{
    public abstract class GenericService<TEntity, TResponseDto>(IUnitOfWork uow, IDtoMapper<TEntity, TResponseDto> mapper) : IGenericService<TEntity, TResponseDto>
        where TEntity : BaseEntity
        where TResponseDto: class

    {
        protected readonly IUnitOfWork UnitOfWork = uow;
        private readonly IGenericRepository<TEntity> _repository = uow.GetRepository<TEntity>();


        public async Task<TResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : mapper.MapToResponseDto(entity);
        }

        public async Task<TResponseDto> CreateAsync(TEntity entity)
        {
            var repository = UnitOfWork.GetRepository<TEntity>();
            var resultCreated = await repository.AddAsync(entity);
            return mapper.MapToResponseDto(resultCreated);
        }

        public async Task UpdateAsync(Guid id, TEntity entity)
        {
            if (entity is null)
            {
                throw new ValidationException("Entity cannot be null.");
            }
            if (entity.Guid == Guid.Empty)
            {
                throw new ValidationException("Entity ID cannot be empty.");
            }

            if (id != entity.Guid)
            {
                throw new BadRequestException("Entity ID is different");
            }
            var repository = UnitOfWork.GetRepository<TEntity>();
            await repository.Update(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var repository = UnitOfWork.GetRepository<TEntity>();
            var entityDeleting = await repository.GetByIdAsync(id);
            if (entityDeleting == null)
            {
                throw new RecordNotFoundException($"Entity with ID {id} not found.");
            }
            await repository.Delete(entityDeleting);
        }
    }

}
