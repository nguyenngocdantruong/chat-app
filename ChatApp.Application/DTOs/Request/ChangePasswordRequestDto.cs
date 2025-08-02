using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Request
{
    public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "Old password is required")]
        public string OldPassword { get; set; } = null!;
        [Required(ErrorMessage = "New NewPassword is required")]
        [StringLength(50, ErrorMessage = "New NewPassword must be between 6 and 50 characters", MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;
    }
}