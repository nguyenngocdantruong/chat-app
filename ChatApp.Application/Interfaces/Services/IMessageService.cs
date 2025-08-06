using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IMessageService: IGenericService<Message, MessageResponseDto>
    {
        Task<PagedResult<MessageResponseDto>> GetMessagesByConversationIdAsync(Guid conversationId, int page = 1, int pageSize = 15);
        Task<Result<MessageResponseDto>> CreateMessageAsync(MessageRequestDto requestDto);
        Task<Result<MessageResponseDto>> UpdateMessageAsync(MessageRequestDto requestDto);
        Task<Result<object>> DeleteMessageAsync(Guid messageId);
        Task<Result<object>> ReadMessageAsync(Guid userId, Guid messageId);


        Task<Result<MessageResponseDto>> GetMessageByIdAsync(Guid id);
    }
}
