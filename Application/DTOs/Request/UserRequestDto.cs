using ChatApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Request
{
    public class UserRequestDto
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

        [InverseProperty("User")]
        public virtual ICollection<ConversationMember> ConversationMembers { get; set; } = new List<ConversationMember>();

        [InverseProperty("User")]
        public virtual ICollection<ConversationSetting> ConversationSettings { get; set; } = new List<ConversationSetting>();

        [InverseProperty("CreatedByNavigation")]
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        [InverseProperty("Addressee")]
        public virtual ICollection<Friend> FriendAddressees { get; set; } = new List<Friend>();

        [InverseProperty("Requester")]
        public virtual ICollection<Friend> FriendRequesters { get; set; } = new List<Friend>();

        [InverseProperty("User")]
        public virtual ICollection<MessageStatus> MessageStatuses { get; set; } = new List<MessageStatus>();

        [InverseProperty("Sender")]
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        [InverseProperty("User")]
        public virtual ICollection<OtpRequest> OtpRequests { get; set; } = new List<OtpRequest>();

        [InverseProperty("User")]
        public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
        [InverseProperty("User")]
        public virtual ICollection<FcmToken> FcmTokens { get; set; } = new List<FcmToken>();
    }
}
