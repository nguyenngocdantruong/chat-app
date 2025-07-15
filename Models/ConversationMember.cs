using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class ConversationMember
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ConversationId { get; set; }

    public Guid? UserId { get; set; }

    [StringLength(20)]
    public string? Role { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinedAt { get; set; }

    [ForeignKey("ConversationId")]
    [InverseProperty("ConversationMembers")]
    public virtual Conversation? Conversation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ConversationMembers")]
    public virtual User? User { get; set; }
}
