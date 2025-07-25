using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IFCMTokenRepository: IGenericRepository<FcmToken>
    {
        Task<List<FcmToken>> GetListFcmTokensByUserAsync(Guid UserId);
        Task UpdateFcmToken(Guid userId, FcmToken fcmToken);

        Task RemoveFcmToken(Guid UserId, FcmToken fcmToken);
        Task RemoveInvalidFcmToken(FcmToken fcmToken);
    }
}
