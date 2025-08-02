using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IMessageService: IGenericService<Message>
    {
        Task<PagedResult<MessageResponseDto>> GetMessagesByConversationIdAsync(Guid conversationId, int page = 1, int pageSize = 15);
        Task<MessageResponseDto> CreateMessageAsync(Guid currentUserId, MessageRequestDto requestDto);
        Task<MessageResponseDto> UpdateMessageAsync(Guid currentUserId, MessageRequestDto requestDto);
        Task<bool> DeleteMessageAsync(Guid currentUserId, Guid messageId);
        Task ReadMessageAsync(Guid userId, Guid messageId);
    }
}
