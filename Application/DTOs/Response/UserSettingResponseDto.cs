using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Domain.Entities
{
    public partial class UserSetting: BaseEntity
    {
        public Guid? UserId { get; set; }

        public bool? MuteAllNotifications { get; set; }

        [StringLength(10)]
        public string? PinSecurityCode { get; set; }

        [Column("EnableE2E")]
        public bool? EnableE2e { get; set; }

        [StringLength(50)]
        public string? ChatTheme { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("UserSettings")]
        public virtual User? User { get; set; }
    }
}


