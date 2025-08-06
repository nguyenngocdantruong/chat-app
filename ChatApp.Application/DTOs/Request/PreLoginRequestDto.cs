using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Request
{
    public class PreLoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid format")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!; 
    }
}
