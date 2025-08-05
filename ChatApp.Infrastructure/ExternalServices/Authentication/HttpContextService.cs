using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Domain.Exceptions.Authentication;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Infrastructure.ExternalServices.Authentication
{
    public class HttpContextService(IHttpContextAccessor context) : ICurrentUserService
    {
        public Guid? UserId
        {
            get
            {
                var userIdString = context.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userIdString, out var userId))
                {
                    return userId;
                }

                throw new UnAuthorizedException("Unauthorized.");
            }
        }

        public string? UserName { get; } = "defaultUser"; // placeholder

        public string? Email
        {
            get
            {
                var emailString = context.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
                return emailString;
            }
        }
    }
}
