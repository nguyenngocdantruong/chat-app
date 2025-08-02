using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Services
{
    public interface IConversationMemberService : IGenericService<ConversationMember>
    {
        Task<bool> IsMemberInConversation(Guid conversationId, Guid userId);
        Task<IEnumerable<ConversationMemberResponseDto>> GetMembersByConversationIdAsync(Guid conversationId);
        Task<ConversationMemberResponseDto> AddMemberToConversationAsync(ConversationEventRequestDto conversationEventRequestDto);
        Task<ConversationMemberResponseDto> UpdateMemberInConversationAsync(ConversationEventRequestDto conversationEventRequestDto);
        Task<bool> RemoveMemberFromConversationAsync(ConversationEventRequestDto conversationEventRequestDto);
    }
}
