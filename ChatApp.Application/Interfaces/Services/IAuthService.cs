using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Enums;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDto>> LoginFirstStep(PreLoginRequestDto loginRequestDto);
        Task<Result<LoginResponseDto>> LoginWith2FaAsync(LoginRequestDto loginRequestDto);
        Task<Result<object>> ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto);
        Task<Result<PreRegisterResponseDto>> PreRegisterAsync(PreRegisterRequestDto preRegisterRequestDto);
        Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto);
        Task<Result<TokenResponseDto>> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto);
        Task<Result<object>> LogoutAsync();
        Task<Result<object>> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto);
        Task<Result<object>> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<Result<LoginResponseDto>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
    }
}
