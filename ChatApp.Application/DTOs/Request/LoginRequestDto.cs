using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Request
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "OTP is required")]
        public string Code { get; set; } = null!; // For 2FA

        [Required(ErrorMessage = "Transaction Id is required")]
        public string TransactionId { get; set; } = null!; // For tracking the login transaction
    }
}
