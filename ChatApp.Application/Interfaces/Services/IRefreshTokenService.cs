using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IRefreshTokenService: IGenericService<RefreshToken>
    {
        Task<IEnumerable<RefreshTokenResponseDto>> GetActiveRefreshTokens(Guid userId);
        Task RevokeAllActiveTokens(Guid userId);
        Task<TokenResponseDto> RotationRefreshToken(RefreshAccessTokenRequestDto requestDto);
    }
}
