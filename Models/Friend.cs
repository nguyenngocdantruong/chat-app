using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class Friend
{
    [Key]
    public Guid Id { get; set; }

    public Guid? RequesterId { get; set; }

    public Guid? AddresseeId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AddresseeId")]
    [InverseProperty("FriendAddressees")]
    public virtual User? Addressee { get; set; }

    [ForeignKey("RequesterId")]
    [InverseProperty("FriendRequesters")]
    public virtual User? Requester { get; set; }
}
