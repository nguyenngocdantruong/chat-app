using System;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Domain.Enums;

namespace ChatApp.Infrastructure.ExternalServices.MailService
{
    public class MailConsoleService : IMailService
    {
        private static Dictionary<string, List<string>> _inbox = [];

        public List<string> GetInbox(string email)
        {
            try
            {
                return _inbox[email];
            }
            catch
            {
                return [];
            }
        }
        private void AddToInbox(string to, string content)
        {
            if (_inbox.TryGetValue(to, out var list))
            {
                list.Add(content);
                _inbox[to] = list;
            }
            else
            {
                _inbox[to] = [content];
            }
        }
        public Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            // Set màu vàng để dễ nhận biết
            Console.ForegroundColor = ConsoleColor.Yellow;
            var value = $"[MAIL] Sending Email to: {to} - Subject: {subject} - Body: {body} - Sent at: {DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd hh:mm:ss")}";
            Console.WriteLine(value);
            AddToInbox(to, value);
            // Trả lại màu mặc định
            Console.ResetColor();
            return Task.CompletedTask;
        }

        public Task SendOtp(string to, string otpCode, ActionType action)
        {
            // Set màu cyan cho OTP để nổi bật hơn
            Console.ForegroundColor = ConsoleColor.Cyan;
            var value = $"[OTP] Sending OTP to: {to} - OTP code: {otpCode} - Action: {action.ToString()} - Sent at: {DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd hh:mm:ss")}";
            Console.WriteLine(value);
            AddToInbox(to, value);
            Console.ResetColor();
            return Task.CompletedTask;
        }
    }
}