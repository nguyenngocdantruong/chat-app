using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User, UserResponseDto>
    {
        Task<Result<UserResponseDto>> GetCurrentUser();
        Task<Result<UserResponseDto>> GetByEmailAsync(string email, bool isFromAuthAction = true);
        Task<Result<UserResponseDto>> GetByUsername(string username, bool isFromAuthAction = true);
        Task<Result<UserResponseDto>> GetByUid(Guid uid, bool isFromAuthAction = true);
        Task<Result<FcmTokenResponseDto>> RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto);
        Task<Result<FcmTokenResponseDto>> UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto);
        Task<bool> ComparePasswordAsync(Guid userId, string password);
        Task<Result<object>> DeleteAccountAsync();

        Task<Result<UserResponseDto>> UpdateProfileAsync(UpdateProfileRequestDto userUpdateRequestDto);
        Task<PagedResult<UserResponseDto>> Filter(UserFilter paginationRequestDto);
    }
}
