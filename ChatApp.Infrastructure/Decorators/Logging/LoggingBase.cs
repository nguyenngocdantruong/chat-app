using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Application.Services;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingBase<TEntity, TResponseDto> : IGenericService<TEntity, TResponseDto> where TEntity : BaseEntity where TResponseDto : BaseResponseDto
    {
        public Task<TResponseDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TResponseDto> CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
