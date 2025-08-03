using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.DTOs.Result;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<LoginResponseDto>> LoginFirstStep(LoginRequestDto loginRequestDto);
        Task<Result<LoginResponseDto>> LoginWith2FaAsync(LoginRequestDto loginRequestDto);
        Task ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto);
        Task<Result<PreRegisterResponseDto>> PreRegisterAsync(PreRegisterRequestDto preRegisterRequestDto);
        Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto);
        Task<Result<TokenResponseDto>> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto);
        Task<Result<object>> LogoutAsync(Guid userId);
        Task<Result<object>> ChangePasswordAsync(Guid currentUserId, ChangePasswordRequestDto changePasswordRequestDto);
        Task<Result<object>> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<Result<LoginResponseDto>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
    }
}
