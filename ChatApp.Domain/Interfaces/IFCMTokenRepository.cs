using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IFcmTokenRepository: IGenericRepository<FcmToken>
    {
        Task<List<FcmToken>> GetListFcmTokensByUserAsync(Guid userId);
        Task UpdateFcmToken(Guid userId, FcmToken fcmToken);

        Task RemoveFcmToken(Guid userId, FcmToken fcmToken);
        Task RemoveInvalidFcmToken(FcmToken fcmToken);
    }
}
