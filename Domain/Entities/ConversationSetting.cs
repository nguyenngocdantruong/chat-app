using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class ConversationSetting : BaseEntity
    {
        public Guid? UserId { get; set; }

        public Guid? ConversationId { get; set; }

        public bool? MuteNotification { get; set; }

        public bool? Pinned { get; set; }

        [ForeignKey("ConversationId")]
        [InverseProperty("ConversationSettings")]
        public virtual Conversation? Conversation { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ConversationSettings")]
        public virtual User? User { get; set; }
    }
}
