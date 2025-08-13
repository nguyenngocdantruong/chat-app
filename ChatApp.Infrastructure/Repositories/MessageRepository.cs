using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }
        public Task<List<Message>> GetMessagesByConversationsAsync(Guid conversationId, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<Message?> GetLastMessageAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetNewMessageAfterASync(Guid conversationId, DateTime lastSeenAt)
        {
            throw new NotImplementedException();
        }

        public Task SetMessageSeenAsync(Guid messageId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasMessageUnseen(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> SearchMessageAsync(Guid conversationId, string searchTerm, int page = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessageForMeAsync(Guid messageId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessageForAllAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetPinnedMessagesAsync(Guid conversationId, int page = 1, int pageSize = 20)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsMessagePinnedAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task PinMessageAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task UnpinMessageAsync(Guid messageId)
        {
            throw new NotImplementedException();
        }

        public Task AddReactionToMessageAsync(Guid messageId, Guid userId, Reaction reaction)
        {
            throw new NotImplementedException();
        }

        public Task RemoveReactionInMessageAsync(Guid messageId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
