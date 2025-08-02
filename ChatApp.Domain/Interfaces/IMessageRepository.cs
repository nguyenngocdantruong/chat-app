using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<List<Message>> GetMessagesByConversationsAsync(Guid conversationId, int page = 1, int pageSize = 20);
        Task<Message?> GetLastMessageAsync(Guid conversationId);
        Task<IEnumerable<Message>> GetNewMessageAfterASync(Guid conversationId, DateTime lastSeenAt);

        Task SetMessageSeenAsync(Guid messageId, Guid userId);
        Task<bool> HasMessageUnseen(Guid conversationId, Guid userId);
        // Tìm kiếm
        Task<IEnumerable<Message>> SearchMessageAsync(Guid conversationId, string searchTerm, int page = 1, int pageSize = 10);

        Task DeleteMessageForMeAsync(Guid messageId, Guid userId);
        Task DeleteMessageForAllAsync(Guid messageId);

        Task<IEnumerable<Message>> GetPinnedMessagesAsync(Guid conversationId, int page = 1, int pageSize = 20);
        Task<bool> IsMessagePinnedAsync(Guid messageId);
        Task PinMessageAsync(Guid messageId);
        Task UnpinMessageAsync(Guid messageId);

        Task AddReactionToMessageAsync(Guid messageId, Guid userId, Reaction reaction);
        Task RemoveReactionInMessageAsync(Guid messageId, Guid userId);
    }
}
