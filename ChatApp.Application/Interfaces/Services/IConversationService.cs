using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IConversationService: IGenericService<Conversation>
    {
        Task<IEnumerable<ConversationResponseDto>> GetConversationsByUserIdAsync(Guid userId, int page = 1, int pageSize = 15);
        Task<ConversationResponseDto> CreateConversationAsync(ConversationCreateRequestDto requestDto);
        Task<ConversationResponseDto> UpdateConversationAsync(ConversationCreateRequestDto requestDto);

        Task<ConversationSettingResponseDto?> GetConversationSettingByUser(Guid conversationId, Guid userId);
    }
}
