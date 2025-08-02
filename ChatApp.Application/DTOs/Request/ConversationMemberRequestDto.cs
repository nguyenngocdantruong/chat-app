using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs.Request
{
    public class ConversationMemberRequestDto
    {
        public Guid? UserId { get; set; }
        public UserRole? Role { get; set; }
        public string? Nickname { get; set; }
    }
}
