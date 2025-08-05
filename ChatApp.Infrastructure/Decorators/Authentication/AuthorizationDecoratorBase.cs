using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Authentication;
using ChatApp.Shared.Common;

namespace ChatApp.Infrastructure.Decorators.Authentication
{
    public abstract class AuthorizationDecoratorBase<TResource>(
        IAuthService authService,
        ICurrentUserService currentUserService,
        IAuthorizationHandler<TResource> authorizationHandler) where TResource : class
    {
        protected IAuthService AuthService { get; private set; } = authService;
        protected ICurrentUserService CurrentUserService { get; private set; } = currentUserService;

        protected async Task<T> ExecuteWithAuthorizationAsync<T>(Func<Task<T>> action, TResource? resource = null, Permission permission = Permission.Read,
            Guid? requestUserGuid = null)
        {
            if (CurrentUserService.UserId == null)
            {
                throw new UnAuthorizedException("User is not authenticated.");
            }

            if (resource != null)
            {
                var isAuthorized = await authorizationHandler.AuthorizeAsync(resource, permission, requestUserGuid);
                if (!isAuthorized)
                {
                    throw new ForbiddenException(
                        $"User is not enough permission to do [{permission.ToString()}] on [{nameof(resource)}]");
                }
            }

            return await action();
        }
    }
}
