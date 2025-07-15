using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Models;

public partial class OtpRequest
{
    [Key]
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    [StringLength(10)]
    public string Code { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime ExpiredAt { get; set; }

    public bool? IsUsed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("OtpRequests")]
    public virtual User? User { get; set; }
}
