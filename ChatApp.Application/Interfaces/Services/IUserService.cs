using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User>
    {
        Task<User> GetCurrentUser(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<FcmToken> RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto);
        Task<FcmToken> UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto);
        Task<bool> ComparePasswordAsync(Guid userId, string password);
        Task DeleteAccountAsync(Guid currentUserId, Guid uid);
    }
}
