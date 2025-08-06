using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IFriendService: IGenericService<Friend, FriendResponseDto>
    {
        Task<PagedResult<FriendResponseDto>> GetFriendsByUserIdAsync(Guid userId, FriendFilter filter);
        Task<Result<FriendResponseDto>> GetFriendBetweenUsers(Guid userId1, Guid userId2);
        Task<Result<FriendResponseDto>> CreateFriendAsync(FriendUpdateRequestDto requestDto);
        Task<Result<FriendResponseDto>> UpdateFriendAsync(FriendUpdateRequestDto requestDto);
        Task<Result<object>> RemoveFriendAsync(Guid friendId);
        Task<PagedResult<FriendResponseDto>> Filter(FriendFilter filter);
    }
}
