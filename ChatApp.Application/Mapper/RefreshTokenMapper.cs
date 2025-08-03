using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Mapper
{
    public class RefreshTokenMapper : IRefreshTokenMapper
    {
        public RefreshTokenResponseDto MapToResponseDto(RefreshToken entity)
        {
            return new RefreshTokenResponseDto
            {
                Guid = entity.Guid,
                Token = entity.Token,
                UserId = entity.UserId,
                ExpirationDate = entity.ExpirationDate,
                IsRevoked = entity.IsRevoked
            };
        }
    }
}
