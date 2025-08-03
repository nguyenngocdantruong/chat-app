using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IConversationService: IGenericService<Conversation, ConversationResponseDto>
    {

        Task<PagedResult<ConversationResponseDto>> GetConversationsByUserIdAsync(Guid userId, ConversationFilter filter);
        Task<ConversationResponseDto> CreateConversationAsync(ConversationCreateRequestDto createRequestDto);
        Task<ConversationResponseDto> UpdateConversationAsync(ConversationUpdateRequestDto updateRequestDto);
        Task<ConversationSettingResponseDto?> GetConversationSettingByUser(Guid conversationId, Guid userId);

        #region Member
        Task<bool> IsMemberInConversation(Guid conversationId, Guid userId);
        Task<IEnumerable<ConversationMemberResponseDto>> GetMembersByConversationIdAsync(Guid conversationId);
        Task<ConversationMemberResponseDto> AddMemberToConversationAsync(ConversationEventRequestDto conversationEventRequestDto);
        Task<ConversationMemberResponseDto> UpdateMemberInConversationAsync(ConversationEventRequestDto conversationEventRequestDto);
        Task<bool> RemoveMemberFromConversationAsync(ConversationEventRequestDto conversationEventRequestDto);
        #endregion
    }
}
