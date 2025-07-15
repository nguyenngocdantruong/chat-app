using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

[Table("MessageStatus")]
public partial class MessageStatus
{
    [Key]
    public Guid Id { get; set; }

    public Guid? MessageId { get; set; }

    public Guid? UserId { get; set; }

    public bool? IsRead { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReadAt { get; set; }

    [ForeignKey("MessageId")]
    [InverseProperty("MessageStatuses")]
    public virtual Message? Message { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("MessageStatuses")]
    public virtual User? User { get; set; }
}
