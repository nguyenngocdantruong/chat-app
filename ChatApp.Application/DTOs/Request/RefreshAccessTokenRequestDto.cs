using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Request
{
    public class RefreshAccessTokenRequestDto : BaseRequestDto
    {
        [Required(ErrorMessage = "User Id is required")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; } = null!;
    }
}
