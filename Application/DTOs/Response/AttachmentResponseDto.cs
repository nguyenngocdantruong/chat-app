using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class Attachment : BaseEntity
    {
        public Guid? MessageId { get; set; }

        [StringLength(2000)]
        public string? FileUrl { get; set; }

        [StringLength(50)]
        public string? FileType { get; set; }

        public long? FileSize { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UploadedAt { get; set; }

        [StringLength(20)]
        public string AttType { get; set; } = null!;

        [ForeignKey("MessageId")]
        [InverseProperty("Attachments")]
        public virtual Message? Message { get; set; }
    }
}