using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Request
{
    public class ConversationEventRequestDto : BaseRequestDto
    {
        public ConverstationEventType Type { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? TargetId { get; set; }
        public object? EventData { get; set; }
    }
}
