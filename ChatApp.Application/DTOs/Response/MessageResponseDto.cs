using ChatApp.Application.DTOs.Request;
using ChatApp.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Response
{
    public partial class MessageResponseDto : BaseResponseDto
    {
        public Guid? ConversationId { get; set; }

        public Guid? SenderId { get; set; }

        public MessageType? MessageType { get; private set; }

        public string? Content { get; set; }

        public DateTime? SentAt { get; set; }

        public AttachmentResponseDto? Attachment { get; set; }

        public bool? IsEdited { get; set; }
        public bool? IsPinned { get; set; }

        public List<MessageStatusResponseDto> MessageStatuses { get; set; } = new List<MessageStatusResponseDto>();
    }
}
