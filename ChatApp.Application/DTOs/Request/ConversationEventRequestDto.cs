using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Request
{
    public class ConversationEventRequestDto<T> : BaseRequestDto
    {
        public ConversationEventType Type { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? TargetId { get; set; }
        public T? EventData { get; set; }
    }
}
