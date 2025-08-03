using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Mapper
{
    public interface IDtoMapper<in TEntity, out TDto> where TEntity : BaseEntity where TDto : class
    {
        TDto MapToResponseDto(TEntity entity);
    }
}
