using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Services
{
    public class FriendService(IUnitOfWork uow, IFriendRepository repository, IFriendMapper mapper) : GenericService<Friend, FriendResponseDto>(uow, repository, mapper), IFriendService
    {
        public Task<PagedResult<FriendResponseDto>> GetFriendsByUserIdAsync(Guid userId, FriendFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<FriendResponseDto>> GetFriendBetweenUsers(Guid userId1, Guid userId2)
        {
            Friend? friend = await repository.GetFriendBetweenUsersAsync(userId1, userId2);
            return new Result<FriendResponseDto>()
            {
                Data = friend == null ? null : mapper.MapToResponseDto(friend),
                IsSuccess = friend != null,
                Message = friend == null
                    ? "No friend relationship found between the users."
                    : "Friend relationship found successfully."
            };
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
