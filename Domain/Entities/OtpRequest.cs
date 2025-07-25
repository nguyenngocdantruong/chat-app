using ChatApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class OtpRequest : BaseEntity
    {
        public Guid? UserId { get; set; }

        [StringLength(10)]
        public string Code { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime ExpiredAt { get; set; }

        public bool? IsUsed { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public ActionType Purpose { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("OtpRequests")]
        public virtual User? User { get; set; }
    }
}

