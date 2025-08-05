using ChatApp.Domain.Enums;

namespace ChatApp.Application.Interfaces.ExternalService
{
    public interface IMailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendOtp(string to, string otpCode, ActionType action);
    }
}
