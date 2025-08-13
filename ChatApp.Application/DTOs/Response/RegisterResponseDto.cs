using ChatApp.Application.DTOs.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Response
{
    public class RegisterResponseDto : BaseResponseDto
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? DisplayName { get; set; }
    }
}
