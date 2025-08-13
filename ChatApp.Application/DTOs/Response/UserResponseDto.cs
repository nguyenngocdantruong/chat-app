using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response 
{
    public class UserResponseDto : BaseResponseDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public AttachmentResponseDto? Avatar { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsSearchable { get; set; } = true;
        public DateTime? LastSeen { get; set; }
        public bool? IsOnline { get; set; }
    }
}

