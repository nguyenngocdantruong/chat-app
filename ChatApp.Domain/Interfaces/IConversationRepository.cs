using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Interfaces
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        Task<IQueryable<Conversation>> GetConversationsByUserIdAsync(Guid userId);
        Task<Conversation?> GetPrivateConversationBetweenUsersAsync(Guid userId1, Guid userId2);
        Task<bool> IsUserInThisConversation(Guid conversationId, Guid userId);
        Task<bool> CreateGroupConversationsAsync(Conversation conversation, List<ConversationMember> members, Guid createdBy);

        Task<IQueryable<ConversationEvent>> GetConversationEventsAsync(Guid conversationId);
        Task<IQueryable<Attachment>> GetConversationAttachmentsAsync(Guid conversationId);
        Task<ConversationSetting?> GetConversationSettingByUserId(Guid conversationId, Guid userId);
        Task<ConversationSetting> CreateDefaultConversationSettingByUserId(Guid conversationId, Guid userId);
        Task<ConversationStatus?> GetConversationStatusAsync(Guid conversationId, Guid userId);
        Task<ConversationStatus> CreateDefaultConversationStatusAsync(Guid conversationId, Guid userId);
    }
}
