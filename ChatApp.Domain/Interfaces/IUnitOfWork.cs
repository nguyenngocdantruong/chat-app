using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAttachmentRepository AttachmentRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
        IConversationRepository ConversationRepository { get; }
        IConversationMemberRepository ConversationMemberRepository { get; }
        IFcmTokenRepository FCMTokenRepository { get; }
        IFriendRepository FriendRepository { get; }
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();
    }
}
