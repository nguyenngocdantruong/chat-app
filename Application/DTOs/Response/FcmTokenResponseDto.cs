using ChatApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public class FcmToken: BaseEntity
    {
        public Guid UserId { get; set; }
        [StringLength(4000)]
        public string Token { get; set; } = null!;
        public string DeviceType { get; set; } = null!;
        public bool IsRevoked { get; set; } = false;
        [ForeignKey("UserId")]
        [InverseProperty("FcmTokens")]
        public virtual User? User { get; set; }
        [NotMapped]
        public DateTime LastUsedAt { get => UpdatedAt; set => UpdatedAt = value; }
    }
}
