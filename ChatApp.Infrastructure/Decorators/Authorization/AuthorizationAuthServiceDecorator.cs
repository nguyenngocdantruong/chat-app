using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Authorization;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Common;

namespace ChatApp.Infrastructure.Decorators.Authorization
{
    public class AuthorizationAuthServiceDecorator(IUserService service, IAuthService authService, ICurrentUserService currentUserService, IAuthorizationHandler<User> handler) : AuthorizationDecoratorBase<User, UserResponseDto>(service, authService, currentUserService, handler), IAuthService
    {
        public Task<Result<LoginResponseDto>> LoginFirstStep(PreLoginRequestDto loginRequestDto)
        {
            return AuthService.LoginFirstStep(loginRequestDto);
        }

        public Task<Result<LoginResponseDto>> LoginWith2FaAsync(LoginRequestDto loginRequestDto)
        {
            return AuthService.LoginWith2FaAsync(loginRequestDto);
        }

        public Task<Result<PreRegisterResponseDto>> PreRegisterAsync(PreRegisterRequestDto preRegisterRequestDto)
        {
            return AuthService.PreRegisterAsync(preRegisterRequestDto);
        }

        public Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto)
        {
            return AuthService.RegisterAsync(userRequestDto, attachmentRequestDto);
        }

        public Task<Result<object>> ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto)
        {
            return AuthService.ResendEmailAsync(resendEmailRequestDto);
        }

        public Task<Result<object>> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            return AuthService.ForgotPasswordAsync(forgotPasswordRequestDto);
        }

        public Task<Result<LoginResponseDto>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            return AuthService.ResetPasswordAsync(resetPasswordRequestDto);
        }

        public Task<Result<TokenResponseDto>> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto)
        {
            return AuthService.RefreshAccessTokenAsync(refreshAccessTokenRequestDto);
        }

        public async Task<Result<object>> LogoutAsync()
        {
            return await ExecuteWithAuthorizationAsync(AuthService.LogoutAsync);
        }

        public Task<Result<object>> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto)
        {
            return ExecuteWithAuthorizationAsync(() => AuthService.ChangePasswordAsync(changePasswordRequestDto));
        }
    }
}