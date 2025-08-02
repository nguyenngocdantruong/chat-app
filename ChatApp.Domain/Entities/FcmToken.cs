using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public class FcmToken: BaseEntity
    {
        public Guid UserId { get; set; }
        [StringLength(4000)]
        public string Token { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public bool IsRevoked { get; set; } = false;
        [NotMapped]
        public DateTime LastUsedAt { get => UpdatedAt; set => UpdatedAt = value; }
    }
}
