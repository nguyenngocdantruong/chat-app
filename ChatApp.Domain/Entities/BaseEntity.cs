using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Guid { get; set; } = Guid.NewGuid();
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        [Column(TypeName = "datetime")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
