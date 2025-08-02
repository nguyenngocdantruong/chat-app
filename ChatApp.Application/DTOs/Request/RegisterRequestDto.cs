using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Application.DTOs.Request
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "NewPassword is required")]
        [StringLength(50)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "DisplayName is required")]
        [StringLength(100)]
        public string? DisplayName { get; set; }
    }
}
