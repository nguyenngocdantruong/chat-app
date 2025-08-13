using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Domain.Enums;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingMailServiceDecorator : IMailService
    {
        public Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            throw new NotImplementedException();
        }

        public Task SendOtp(string to, string otpCode, ActionType action)
        {
            throw new NotImplementedException();
        }
    }
}
