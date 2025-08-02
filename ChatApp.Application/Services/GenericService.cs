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
    public abstract class GenericService<TEntity>(IUnitOfWork uow) : IGenericService<TEntity>
        where TEntity : BaseEntity

    {
        protected readonly IUnitOfWork UnitOfWork = uow;
        private readonly IGenericRepository<TEntity> _repository = uow.GetRepository<TEntity>();


        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var repository = UnitOfWork.GetRepository<TEntity>();
            var resultCreated = await repository.AddAsync(entity);
            return resultCreated;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ValidationException("Entity cannot be null.");
            }
            if (entity.Guid == Guid.Empty)
            {
                throw new ValidationException("Entity ID cannot be empty.");
            }
            var repository = UnitOfWork.GetRepository<TEntity>();
            return await repository.Update(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var repository = UnitOfWork.GetRepository<TEntity>();
            var entityDeleting = await repository.GetByIdAsync(id);
            if (entityDeleting == null)
            {
                throw new RecordNotFoundException($"Entity with ID {id} not found.");
            }
            var resultDeleted = await repository.Delete(entityDeleting);
            return resultDeleted;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }

}
