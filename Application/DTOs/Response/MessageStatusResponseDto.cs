using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public class MessageStatus : BaseEntity
    {

        public Guid? MessageId { get; set; }

        [ForeignKey(nameof(MessageId))]
        public virtual Message? Message { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        public Reaction Reaction { get; set; }
    }
}
