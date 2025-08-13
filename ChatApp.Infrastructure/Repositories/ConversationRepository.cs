using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class ConversationRepository(AppDbContext context)
        : GenericRepository<Conversation>(context), IConversationRepository
    {
        public Task<IQueryable<Conversation>> GetConversationsByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Conversation?> GetPrivateConversationBetweenUsersAsync(Guid userId1, Guid userId2)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInThisConversation(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateGroupConversationsAsync(Conversation conversation, List<ConversationMember> members)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ConversationEvent>> GetConversationEventsAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Attachment>> GetConversationAttachmentsAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationSetting> GetConversationSettingByUserId(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ConversationStatus> GetConversationStatusAsync(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
