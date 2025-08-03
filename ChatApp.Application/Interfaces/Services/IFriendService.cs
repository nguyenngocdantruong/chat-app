using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IFriendService: IGenericService<Friend, FriendResponseDto>
    {
        Task<IQueryable<FriendResponseDto>> GetFriendsByUserIdAsync(Guid userId, FriendFilter filter);
        Task<FriendResponseDto?> GetFriendBetweenUsers(Guid userId1, Guid userId2);
        Task<FriendResponseDto> CreateFriendAsync(FriendUpdateRequestDto requestDto);
        Task<FriendResponseDto> UpdateFriendAsync(FriendUpdateRequestDto requestDto);
        Task<bool> RemoveFriendAsync(Guid friendId);
    }
}
