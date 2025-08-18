using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class MessageRepository(AppDbContext context) : GenericRepository<Message>(context), IMessageRepository
    {
        public Task<List<Message>> GetMessagesByConversationsAsync(Guid conversationId, int page = 1, int pageSize = 20)
        {
            var messages = DbSet
                .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return messages;
        }

        public async Task<Message?> GetLastMessageAsync(Guid conversationId)
        {
            return await DbSet.Where(m => m.ConversationId == conversationId && !m.IsDeleted).OrderByDescending(m => m.CreatedAt).Take(1).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Message>> GetNewMessageAfterASync(Guid conversationId, DateTime lastSeenAt)
        {
            var messages = await DbSet
                .Where(m => m.ConversationId == conversationId && m.SentAt > lastSeenAt && !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
            return messages;
        }

        public async Task SetMessageSeenAsync(Guid messageId, Guid userId)
        {
            var oldStatus = await context.MessageStatuses
                .FirstOrDefaultAsync(ms => ms.MessageId == messageId && ms.UserId == userId);
            if (oldStatus != null)
            {
                oldStatus.IsRead = true;
            }
            else
            {
                var newStatus = new MessageStatus
                {
                    MessageId = messageId,
                    UserId = userId,
                    IsRead = true,
                    Reaction = Reaction.None
                };
                context.MessageStatuses.Add(newStatus);
            }
        }

        public async Task<bool> HasMessageUnseen(Guid conversationId, Guid userId)
        {
            var lastMessage = await GetLastMessageAsync(conversationId);
            if (lastMessage == null)
            {
                return false; // No messages in the conversation
            }
            var lastStatus = await context.MessageStatuses
                .Where(ms => ms.MessageId == lastMessage.Guid && ms.UserId == userId)
                .FirstOrDefaultAsync();
            return lastStatus == null || !lastStatus.IsRead;
        }

        public async Task<IEnumerable<Message>> SearchMessageAsync(Guid conversationId, string searchTerm, int page = 1, int pageSize = 10)
        {
            var messages = await DbSet
                .Where(m => m.ConversationId == conversationId && m.Content.Contains(searchTerm) && !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return messages;
        }

        public async Task DeleteMessageForMeAsync(Guid messageId, Guid userId)
        {
            var message = DbSet.FirstOrDefaultAsync(m => m.Guid == messageId && !m.IsDeleted);
            if (message == null)
            {
                throw new RecordNotFoundException("Message not found.");
            }
            var status = await context.MessageStatuses
                .FirstOrDefaultAsync(ms => ms.MessageId == messageId && ms.UserId == userId);
            if (status != null)
            {
                status.IsDeleted = true;
            }
            else
            {
                var newStatus = new MessageStatus
                {
                    MessageId = messageId,
                    UserId = userId,
                    IsRead = false,
                    IsDeleted = true,
                    Reaction = Reaction.None
                };
                context.MessageStatuses.Add(newStatus);
            }
        }

        public async Task DeleteMessageForAllAsync(Guid messageId)
        {
            var message = await DbSet.FirstOrDefaultAsync(m => m.Guid == messageId && !m.IsDeleted);
            if (message == null)
            {
                throw new RecordNotFoundException("Message not found.");
            }
            message.IsDeleted = true;
        }

        public async Task<IEnumerable<Message>> GetPinnedMessagesAsync(Guid conversationId, int page = 1, int pageSize = 20)
        {
            var pinnedMessages = await DbSet
                .Where(m => m.ConversationId == conversationId && m.IsPinned == true && !m.IsDeleted)
                .OrderByDescending(m => m.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return pinnedMessages;
        }

        public Task<bool> IsMessagePinnedAsync(Guid messageId)
        {
            return DbSet.AnyAsync(m => m.Guid == messageId && m.IsPinned == true && !m.IsDeleted);
        }

        public async Task PinMessageAsync(Guid messageId)
        {
            var message = await DbSet.FirstOrDefaultAsync(m => m.Guid == messageId && !m.IsDeleted);
            if (message == null)
            {
                throw new RecordNotFoundException("Message not found.");
            }
            message.IsPinned = true;
        }

        public async Task UnpinMessageAsync(Guid messageId)
        {
            var message = await DbSet.FirstOrDefaultAsync(m => m.Guid == messageId && !m.IsDeleted);
            if (message == null)
            {
                throw new RecordNotFoundException("Message not found.");
            }
            message.IsPinned = false;
        }

        public async Task AddReactionToMessageAsync(Guid messageId, Guid userId, Reaction reaction)
        {
            var message = await DbSet.FirstOrDefaultAsync(m => m.Guid == messageId && !m.IsDeleted);
            if (message == null)
            {
                throw new RecordNotFoundException("Message not found.");
            }
            var status = await context.MessageStatuses
                .FirstOrDefaultAsync(ms => ms.MessageId == messageId && ms.UserId == userId);
            if (status != null)
            {
                status.Reaction = reaction;
            }
            else
            {
                var newStatus = new MessageStatus
                {
                    MessageId = messageId,
                    UserId = userId,
                    IsRead = false,
                    Reaction = reaction
                };
                context.MessageStatuses.Add(newStatus);
            }
        }

        public async Task RemoveReactionInMessageAsync(Guid messageId, Guid userId)
        {
            var status = await context.MessageStatuses
                .FirstOrDefaultAsync(ms => ms.MessageId == messageId && ms.UserId == userId);
            if (status != null)
            {
                status.Reaction = Reaction.None;
            }
            else
            {
                throw new RecordNotFoundException("Reaction not found for this message.");
            }
        }
    }
}
