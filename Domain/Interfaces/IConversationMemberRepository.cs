using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Interfaces
{
    public interface IConversationMemberRepository: IGenericRepository<ConversationMember>
    {
        Task<IEnumerable<User>> GetMembersAsync(Guid conversationId);
        Task AddMemberAsync(Guid conversationId, Guid userId);
        Task<bool> UpdateMemberRoleAsync(Guid conversationId, Guid userId, UserRole role);
        Task RemoveMemberAsync(Guid conversationId, Guid userId); 
    }
}
