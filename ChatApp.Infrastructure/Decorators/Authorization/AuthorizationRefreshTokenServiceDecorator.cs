using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;

namespace ChatApp.Infrastructure.Decorators.Authorization
{
    internal class AuthorizationRefreshTokenServiceDecorator(IRefreshTokenService service, IAuthService authService, ICurrentUserService currentUserService, IAuthorizationHandler<RefreshToken> authorizationHandler) : AuthorizationDecoratorBase<RefreshToken, RefreshTokenResponseDto>(service, authService, currentUserService, authorizationHandler), IRefreshTokenService
    {
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
