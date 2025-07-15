using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class ConversationSetting
{
    [Key]
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ConversationId { get; set; }

    public bool? MuteNotification { get; set; }

    public bool? Pinned { get; set; }

    [StringLength(20)]
    public string? CustomColor { get; set; }

    [ForeignKey("ConversationId")]
    [InverseProperty("ConversationSettings")]
    public virtual Conversation? Conversation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ConversationSettings")]
    public virtual User? User { get; set; }
}
