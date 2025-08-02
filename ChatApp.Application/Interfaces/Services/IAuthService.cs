using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginFirstStep(LoginRequestDto loginRequestDto);
        Task<LoginResponseDto> LoginWith2FaAsync(LoginRequestDto loginRequestDto);
        Task ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto);
        Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto);
        Task<TokenResponseDto> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto);
        Task LogoutAsync(Guid userId);
        Task<bool> ChangePasswordAsync(Guid currentUserId, ChangePasswordRequestDto changePasswordRequestDto);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<LoginResponseDto> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
    }
}
