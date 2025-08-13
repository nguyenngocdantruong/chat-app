using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.AuthorizationHandler
{
    public class AuthorizationFcmTokenHandler : IAuthorizationHandler<FcmToken>
    {
        public Task<bool> AuthorizeAsync(FcmToken resource, Permission permission, Guid? requestUserGuid = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthorizeAsync(Permission permission, Guid? requestUserGuid = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthorizeAsync(Guid idResource, Permission permission, Guid? requestUserGuid = null)
        {
            throw new NotImplementedException();
        }
    }
}
