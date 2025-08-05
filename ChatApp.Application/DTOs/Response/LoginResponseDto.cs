using ChatApp.Application.DTOs.Request;

namespace ChatApp.Application.DTOs.Response
{
    public class LoginResponseDto
    {
        public TokenResponseDto? Token { get; set; }
        public UserResponseDto? User { get; set; }
        public string? TransactionId { get; set; }
    }
}
