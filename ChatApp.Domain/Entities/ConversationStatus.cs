using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Entities
{
    public class ConversationStatus: BaseEntity
    {
        public Guid? ConversationId { get; set; }
        public Guid? UserId { get; set; }
    }
}
