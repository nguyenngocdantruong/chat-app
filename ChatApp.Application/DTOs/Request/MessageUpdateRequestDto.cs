using ChatApp.Domain.Entities;
using MsgType = ChatApp.Domain.Enums.MessageType;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Application.DTOs.Request
{
    public class MessageUpdateRequestDto: BaseRequestDto
    {
        public string? Content { get; set; }
        public AttachmentRequestDto? Attachment { get; set; }
    }
}
