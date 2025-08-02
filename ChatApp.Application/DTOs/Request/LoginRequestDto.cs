using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Request
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid format")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Code { get; set; } // For 2FA
        public string? TransactionId { get; set; } // For tracking the login transaction
        public FcmTokenRequestDto? FcmToken { get; set; } // For storing the FCM token
    }
}
