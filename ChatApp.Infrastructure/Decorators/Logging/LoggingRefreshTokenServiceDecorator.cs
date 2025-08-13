using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingRefreshTokenServiceDecorator : LoggingBase<Friend, FriendResponseDto>, IRefreshTokenService
    {
        public Task<RefreshTokenResponseDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenResponseDto> CreateAsync(RefreshToken entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, RefreshToken entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RefreshTokenResponseDto>> GetActiveRefreshTokens(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RevokeAllActiveTokens(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RevokeToken(Guid tokenId)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponseDto> RotationRefreshToken(RefreshAccessTokenRequestDto requestDto)
        {
            throw new NotImplementedException();
        }
    }
}
