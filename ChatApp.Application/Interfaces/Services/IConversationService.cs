using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Common;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IConversationService: IGenericService<Conversation, ConversationResponseDto>
    {

        Task<PagedResult<ConversationListItemResponseDto>> GetConversationsByUserIdAsync(ConversationFilter filter);
        Task<Result<ConversationResponseDto>> CreateConversationAsync(ConversationCreateRequestDto createRequestDto);
        Task<Result<ConversationResponseDto>> UpdateConversationAsync(ConversationUpdateRequestDto updateRequestDto);
        Task<Result<Result<ConversationSettingResponseDto>>> GetConversationSettingByUser(Guid conversationId, Guid userId);

        #region Member
        Task<Result<object>> IsMemberInConversation(Guid conversationId, Guid userId);
        Task<PagedResult<ConversationMemberResponseDto>> GetMembersByConversationIdAsync(Guid conversationId);
        Task<Result<ConversationMemberResponseDto>> UpdateMemberConversationAsync(ConversationEventRequestDto<ConversationMemberRequestDto> conversationEventRequestDto);
        #endregion

        Task<Result<ConversationResponseDto>> GetConversationById(Guid conversationId);
        Task<Result<object>> DeleteConversationForUserAsync(Guid conversationId);
    }
}
