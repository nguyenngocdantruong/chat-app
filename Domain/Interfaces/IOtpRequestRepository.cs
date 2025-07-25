using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Interfaces
{
    public interface IOtpRequestRepository : IGenericRepository<OtpRequest>
    {
        Task<OtpRequest?> GetLastOtpRequest(Guid userId, ActionType otpType);
        Task<bool> IsOtpRequestValid(Guid userId, ActionType otpType, string otpCode, TimeSpan? validDuration = null);
        Task<bool> MarkAsInvalidOtp(Guid otpId);
        Task<int> CountRecentOtpRequestAsync(Guid userId, int minute);
        Task<int> DeleteExpiredOtpsAsync();
    }
}
