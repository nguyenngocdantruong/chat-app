using ChatApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class ConversationMember : BaseEntity
    {
        public Guid? ConversationId { get; set; }

        public Guid? UserId { get; set; }

        public UserRole? Role { get; set; }

        public string? Nickname { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? JoinedAt { get; set; }
    }
}
