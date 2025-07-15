using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class Conversation
{
    [Key]
    public Guid Id { get; set; }

    public bool? IsGroup { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    public Guid? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Conversation")]
    public virtual ICollection<ConversationMember> ConversationMembers { get; set; } = new List<ConversationMember>();

    [InverseProperty("Conversation")]
    public virtual ICollection<ConversationSetting> ConversationSettings { get; set; } = new List<ConversationSetting>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("Conversations")]
    public virtual User? CreatedByNavigation { get; set; }

    [InverseProperty("Conversation")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
