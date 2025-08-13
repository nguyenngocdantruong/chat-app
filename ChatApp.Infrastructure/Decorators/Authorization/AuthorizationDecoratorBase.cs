using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Authentication;
using ChatApp.Domain.Interfaces;
using System.Security;

namespace ChatApp.Infrastructure.Decorators.Authorization
{
    public abstract class AuthorizationDecoratorBase<TResource, TResponseDto>(
        IGenericService<TResource, TResponseDto> decoratedService,
        IAuthService authService,
        ICurrentUserService currentUserService,
        IAuthorizationHandler<TResource> authorizationHandler) : IGenericService<TResource, TResponseDto> where TResource : BaseEntity where TResponseDto : BaseResponseDto
    {
        protected IAuthService AuthService { get; private set; } = authService;
        protected ICurrentUserService CurrentUserService { get; private set; } = currentUserService;

        private void CheckAuthentication()
        {
            if (CurrentUserService.UserId == null)
            {
                throw new UnAuthorizedException("User is not authenticated.");
            }
        }

        private async Task CheckAuthorization(TResource? resource, Permission permission, Guid? requestUserGuid)
        {
            bool isAuthorized = resource != null ?
                await authorizationHandler.AuthorizeAsync(resource, permission, requestUserGuid) :
                await authorizationHandler.AuthorizeAsync(permission, requestUserGuid);
            if (!isAuthorized)
            {
                throw new ForbiddenException(
                    $"User is not enough permission to do [{permission.ToString()}] on [{typeof(TResource).Name}]");
            }
        }

        private async Task CheckAuthorization(Guid idResource, Permission permission, Guid? requestUserGuid)
        {
            var isAuthorized = await authorizationHandler.AuthorizeAsync(idResource, permission, requestUserGuid);
            if (!isAuthorized)
            {
                throw new ForbiddenException(
                    $"User is not enough permission to do [{permission.ToString()}] on [{typeof(TResource).Name}]");
            }
        }

        protected async Task<T> ExecuteWithAuthorizationAsync<T>(Func<Task<T>> action, TResource? resource = null, Permission permission = Permission.Read,
            Guid? requestUserGuid = null)
        {
            CheckAuthentication();
            await CheckAuthorization(resource, permission, requestUserGuid);
            return await action();
        }

        protected async Task ExecuteWithAuthorizationAsync(Func<Task> action, TResource? resource = null, Permission permission = Permission.Read,
            Guid? requestUserGuid = null)
        {
            CheckAuthentication();
            await CheckAuthorization(resource, permission, requestUserGuid);
            await action();
        }

        protected async Task<T> ExecuteWithAuthorizationWithIdAsync<T>(Func<Task<T>> action, Guid resource, Permission permission = Permission.Read,
            Guid? requestUserGuid = null)
        {
            CheckAuthentication();
            await CheckAuthorization(resource, permission, requestUserGuid);
            return await action();
        }

        protected async Task ExecuteWithAuthorizationWithIdAsync(Func<Task> action, Guid resource, Permission permission = Permission.Read,
            Guid? requestUserGuid = null)
        {
            CheckAuthentication();
            await CheckAuthorization(resource, permission, requestUserGuid);
            await action();
        }

        public Task<TResponseDto?> GetByIdAsync(Guid id)
        {
            return ExecuteWithAuthorizationAsync(() => decoratedService.GetByIdAsync(id));
        }

        public Task<TResponseDto> CreateAsync(TResource entity)
        {
            return ExecuteWithAuthorizationAsync(() => decoratedService.CreateAsync(entity), entity, Permission.Create);
        }

        public Task UpdateAsync(Guid id, TResource entity)
        {
            return ExecuteWithAuthorizationAsync(() => decoratedService.UpdateAsync(id, entity), entity,
                Permission.Update);
        }

        public async Task DeleteAsync(Guid id)
        {
            await ExecuteWithAuthorizationWithIdAsync(() => decoratedService.DeleteAsync(id), id,
                Permission.Delete);
        }
    }
}
