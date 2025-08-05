using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class Conversation : BaseEntity
    {
        public bool? IsGroup { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        [StringLength(50)]
        public string? Theme { get; set; }

        [StringLength(50)]
        public string? Emoji { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}