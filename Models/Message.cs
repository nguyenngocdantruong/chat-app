using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class Message
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ConversationId { get; set; }

    public Guid? SenderId { get; set; }

    [StringLength(20)]
    public string? MessageType { get; set; }

    public string? Content { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsEdited { get; set; }

    [InverseProperty("Message")]
    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    [ForeignKey("ConversationId")]
    [InverseProperty("Messages")]
    public virtual Conversation? Conversation { get; set; }

    [InverseProperty("Message")]
    public virtual ICollection<MessageStatus> MessageStatuses { get; set; } = new List<MessageStatus>();

    [ForeignKey("SenderId")]
    [InverseProperty("Messages")]
    public virtual User? Sender { get; set; }
}
