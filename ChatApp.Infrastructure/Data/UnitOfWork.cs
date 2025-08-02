using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using ChatApp.Domain.Exceptions.Database;
using ArgumentNullException = ChatApp.Domain.Exceptions.Runtime.ArgumentNullException; // Added for Dictionary

namespace ChatApp.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAttachmentRepository AttachmentRepository { get; private set; }
        public IAuditLogRepository AuditLogRepository { get; private set; }
        public IConversationRepository ConversationRepository { get; private set; }
        public IConversationMemberRepository ConversationMemberRepository { get; private set; }
        public IFcmTokenRepository FCMTokenRepository { get; private set; }
        public IFriendRepository FriendRepository { get; private set; }
        public IMessageRepository MessageRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; private set; }

        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(
            AppDbContext context,
            //IAttachmentRepository attachmentRepository,
            IAuditLogRepository auditLogRepository,
            //IConversationRepository conversationRepository,
            //IConversationMemberRepository conversationMemberRepository,
            //IFcmTokenRepository fcmTokenRepository,
            //IFriendRepository friendRepository,
            //IMessageRepository messageRepository,
            //IOtpRequestRepository otpRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository
        )
        {
            _context = context;
            //AttachmentRepository = attachmentRepository;
            AuditLogRepository = auditLogRepository;
            //ConversationRepository = conversationRepository;
            //ConversationMemberRepository = conversationMemberRepository;
            //FCMTokenRepository = fcmTokenRepository;
            //FriendRepository = friendRepository;
            //MessageRepository = messageRepository;
            //OtpRepository = otpRepository;
            RefreshTokenRepository = refreshTokenRepository;
            UserRepository = userRepository;

            _repositories = new Dictionary<Type, object>
            {
                //[typeof(Attachment)] = AttachmentRepository,
                [typeof(AuditLog)] = AuditLogRepository,
                //[typeof(Conversation)] = ConversationRepository,
                //[typeof(ConversationMember)] = ConversationMemberRepository,
                //[typeof(FcmToken)] = FCMTokenRepository,
                //[typeof(Friend)] = FriendRepository,
                //[typeof(Message)] = MessageRepository,
                //[typeof(OtpRequest)] = OtpRepository,
                [typeof(RefreshToken)] = RefreshTokenRepository,
                [typeof(User)] = UserRepository
            };
        }
        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            
            if (_repositories.TryGetValue(type, out var repo))
            {
                return (IGenericRepository<T>)repo;
            }

            throw new ArgumentNullException($"Repository not found.", type.Name);
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new DatabaseOperationException($"An error occurred while saving changes to the database: {ex.Message}");
            }
        }

    }
}
