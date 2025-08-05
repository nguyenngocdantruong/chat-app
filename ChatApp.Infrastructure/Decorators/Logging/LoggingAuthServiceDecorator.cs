using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Shared.Common;
using System;
using System.Threading.Tasks;
using ChatApp.Domain.Exceptions.Authentication;

namespace ChatApp.Infrastructure.Decorators.Logging
{
    public class LoggingAuthServiceDecorator(
        IAuthService authService,
        IAuditLogService auditLogService,
        ICurrentUserService currentUserService) : IAuthService
    {
        public Task<Result<LoginResponseDto>> LoginFirstStep(LoginRequestDto loginRequestDto)
        {
            auditLogService.SaveLogAsync("LoginAttempt", null, null, "User attempted to log in.");
            return authService.LoginFirstStep(loginRequestDto);
        }

        public Task<Result<LoginResponseDto>> LoginWith2FaAsync(LoginRequestDto loginRequestDto)
        {
            auditLogService.SaveLogAsync("Login2FA", null, null, "User attempted to log in with 2FA.");
            return authService.LoginWith2FaAsync(loginRequestDto);
        }

        public async Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto)
        {
            var result = await authService.RegisterAsync(userRequestDto, attachmentRequestDto);

            if (result is { IsSuccess: true, Data.User: not null })
            {
                await auditLogService.SaveLogAsync("UserRegistered", result.Data.User.Guid, result.Data.User.Guid, "A new user has registered.");
            }
            return result;
        }

        public async Task<Result<object>> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto)
        {
            var userId = changePasswordRequestDto.UserId;
            await auditLogService.SaveLogAsync("ChangePasswordAttempt",userId, userId, "User attempted to change password.");
            var result = await authService.ChangePasswordAsync(changePasswordRequestDto);
            if (result.IsSuccess)
            {
                await auditLogService.SaveLogAsync("ChangePasswordSuccess", userId, null, "User successfully changed password.");
            }
            return result;
        }

        public Task<Result<object>> LogoutAsync()
        {
            var userId = currentUserService.UserId;
            auditLogService.SaveLogAsync("Logout", userId, null, "User logged out.");
            return authService.LogoutAsync();
        }

        public Task<Result<object>> ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto)
        {
            auditLogService.SaveLogAsync("Send email otp", null, null, resendEmailRequestDto.Email);
            return authService.ResendEmailAsync(resendEmailRequestDto);
        }

        public Task<Result<PreRegisterResponseDto>> PreRegisterAsync(PreRegisterRequestDto preRegisterRequestDto)
        {
            auditLogService.SaveLogAsync("Pre-register", null, null, preRegisterRequestDto.Email);
            return authService.PreRegisterAsync(preRegisterRequestDto);
        }

        public Task<Result<TokenResponseDto>> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto)
        {
            return authService.RefreshAccessTokenAsync(refreshAccessTokenRequestDto);
        }

        public Task<Result<object>> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            return authService.ForgotPasswordAsync(forgotPasswordRequestDto);
        }

        public Task<Result<LoginResponseDto>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            return authService.ResetPasswordAsync(resetPasswordRequestDto);
        }
    }
}
