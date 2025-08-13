using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingFriendServiceDecorator : LoggingBase<Friend, FriendResponseDto>, IFriendService
    {
        public Task<PagedResult<FriendResponseDto>> GetFriendsByUserIdAsync(Guid userId, FriendFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<Result<FriendResponseDto>> GetFriendBetweenUsers(Guid userId1, Guid userId2)
        {
            throw new NotImplementedException();
        }

        public Task<Result<FriendResponseDto>> CreateFriendAsync(FriendUpdateRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<FriendResponseDto>> UpdateFriendAsync(FriendUpdateRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<object>> RemoveFriendAsync(Guid friendId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<FriendResponseDto>> Filter(FriendFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
