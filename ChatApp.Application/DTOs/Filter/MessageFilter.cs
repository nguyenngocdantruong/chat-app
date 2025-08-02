using System.ComponentModel.DataAnnotations;

namespace ChatApp.Application.DTOs.Filter
{
    public class MessageFilter: BaseFilter
    {
        public Guid? ConversationId { get; set; }
        public Guid? SenderId { get; set; }
        public bool? IsPinned { get; set; }
    }
}
