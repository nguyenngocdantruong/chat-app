using ChatApp.Domain.Entities;
using MsgType = ChatApp.Domain.Enums.MessageType;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Request
{
    public class MessageRequestDto: BaseRequestDto
    {
        public Guid? ConversationId { get; set; }

        public Guid? SenderId { get; set; }

        public MsgType? MessageType { get; set; } = MsgType.Text;

        public string? Content { get; set; }

        public AttachmentRequestDto? Attachment { get; set; }
    }
}
