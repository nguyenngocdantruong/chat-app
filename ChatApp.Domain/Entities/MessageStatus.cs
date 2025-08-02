using ChatApp.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public class MessageStatus : BaseEntity
    {
        public Guid? MessageId { get; set; }

        public Guid? UserId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        public Reaction Reaction { get; set; }
    }
}
