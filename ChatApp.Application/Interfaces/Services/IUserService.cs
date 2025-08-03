using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User, UserResponseDto>
    {
        Task<UserResponseDto> GetCurrentUser(Guid userId);
        Task<UserResponseDto?> GetByEmailAsync(string email);
        Task<UserResponseDto?> GetByUsername(string username);
        Task<FcmTokenResponseDto> RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto);
        Task<FcmTokenResponseDto> UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto);
        Task<bool> ComparePasswordAsync(Guid userId, string password);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto changePasswordRequestDto);
        Task ResetPasswordAsync(Guid userId, ResetPasswordRequestDto resetPasswordRequestDto);
        Task DeleteAccountAsync(Guid currentUserId, Guid uid);
    }
}
