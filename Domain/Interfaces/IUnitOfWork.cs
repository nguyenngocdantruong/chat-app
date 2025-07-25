namespace ChatApp.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAttachmentRepository AttachmentRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
        IConversationRepository ConversationRepository { get; }
        IConversationMemberRepository ConversationMemberRepository { get; }
        IFCMTokenRepository FCMTokenRepository { get; }
        IFriendRepository FriendRepository { get; }
        IMessageRepository MessageRepository { get; }
        IOtpRequestRepository OtpRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
