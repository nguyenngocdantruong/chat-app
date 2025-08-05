using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.Interfaces.Authorization
{
    public interface IAuthorizationHandler<in TResource> where TResource : class
    {
        Task<bool> AuthorizeAsync(TResource resource, Permission permission, Guid? requestUserGuid = null);
    }
}
