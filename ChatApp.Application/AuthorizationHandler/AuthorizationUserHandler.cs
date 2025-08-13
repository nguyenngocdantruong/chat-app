using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.AuthorizationHandler
{
    public class AuthorizationUserHandler : IAuthorizationHandler<User>
    {
        public Task<bool> AuthorizeAsync(User resource, Permission permission, Guid? requestUserGuid = null)
        {
            switch (permission)
            {
                case Permission.Read:
                    // Allow read access if the user is searchable or if the request is for the user themselves
                    return Task.FromResult(resource.IsSearchable.HasValue && resource.IsSearchable.Value || 
                                           (requestUserGuid.HasValue && resource.Guid == requestUserGuid.Value));
                case Permission.Update:
                    // Allow update access only if the request is for the user themselves
                    return Task.FromResult(requestUserGuid.HasValue && resource.Guid == requestUserGuid.Value);
                case Permission.Delete:
                    // Allow deletes access only if the request is for the user themselves
                    return Task.FromResult(requestUserGuid.HasValue && resource.Guid == requestUserGuid.Value);
                default:
                    return Task.FromResult(false);
            }
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
