using ChatApp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public class ConversationStatus: BaseEntity
    {
        public Guid? ConversationId { get; set; }
        public Guid? UserId { get; set; }

        [ForeignKey("ConversationId")]
        [InverseProperty("ConversationStatuses")]
        public virtual Conversation? Conversation { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ConversationStatuses")]
        public virtual User? User { get; set; }
    }
}
