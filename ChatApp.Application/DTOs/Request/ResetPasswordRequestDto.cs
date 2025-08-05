using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Request
{
    public class ResetPasswordRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Reset token is required")]
        public string ResetToken { get; set; } = null!;
        
        [Required(ErrorMessage = "New password is required")]
        [StringLength(50, ErrorMessage = "New NewPassword must be between 6 and 50 characters", MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;
    }
}