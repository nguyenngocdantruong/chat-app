using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    [Index("Username", Name = "UQ__Users__536C85E465DDA45B", IsUnique = true)]
    [Index("Email", Name = "UQ__Users__A9D1053412A3023E", IsUnique = true)]
    public partial class User: BaseEntity
    {
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [StringLength(100)]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        [StringLength(100)]
        public string? DisplayName { get; set; }

        public bool? IsSearchable { get; set; } = true;

        [Column(TypeName = "datetime")]
        public DateTime? LastSeen { get; set; }

        public bool? IsOnline { get; set; }
    }
}

