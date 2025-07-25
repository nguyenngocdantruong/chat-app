using ChatApp.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public partial class Message : BaseEntity
    {
        public Guid? ConversationId { get; set; }

        public Guid? SenderId { get; set; }

        public MessageType? MessageType { get; private set; }

        public string? Content { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SentAt { get; set; }

        public bool? IsEdited { get; set; }
        public bool? IsPinned { get; set; }

        [InverseProperty("Message")]
        public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

        [ForeignKey("ConversationId")]
        [InverseProperty("Messages")]
        public virtual Conversation? Conversation { get; set; }

        [InverseProperty("Message")]
        public virtual ICollection<MessageStatus> MessageStatuses { get; set; } = new List<MessageStatus>();

        [ForeignKey("SenderId")]
        [InverseProperty("Messages")]
        public virtual User? Sender { get; set; }
    }
}
