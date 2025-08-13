using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Infrastructure.Decorators.Authorization
{
    internal class AuthorizationUserServiceDecorator(IUserService service, IAuthService authService, ICurrentUserService currentUserService, IAuthorizationHandler<User> authorizationHandler) : AuthorizationDecoratorBase<User, UserResponseDto>(service, authService, currentUserService, authorizationHandler), IUserService
    {
        public Task<Result<UserResponseDto>> GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserResponseDto>> GetByEmailAsync(string email, bool isFromAuthAction = true)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserResponseDto>> GetByUsername(string username, bool isFromAuthAction = true)
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserResponseDto>> GetByUid(Guid uid, bool isFromAuthAction = true)
        {
            throw new NotImplementedException();
        }

        public Task<Result<FcmTokenResponseDto>> RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<FcmTokenResponseDto>> UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ComparePasswordAsync(Guid userId, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> DeleteAccountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserResponseDto>> UpdateProfileAsync(UpdateProfileRequestDto userUpdateRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<UserResponseDto>> Filter(UserFilter paginationRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
