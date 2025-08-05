using System.ComponentModel.DataAnnotations;
using ChatApp.Domain.Enums;
namespace ChatApp.Domain.Entities
{
    public class AuditLog: BaseEntity
    {
        public Guid? UserId { get; set; }
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;
        public Guid? TargetId { get; set; }
        [StringLength(100)] 
        public string? Note { get; set; } = string.Empty;
    }
}
