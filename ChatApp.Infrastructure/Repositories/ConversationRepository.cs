using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class ConversationRepository(AppDbContext context)
        : GenericRepository<Conversation>(context), IConversationRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IQueryable<Conversation>> GetConversationsByUserIdAsync(Guid userId)
        {
            var listId = await _context.ConversationMembers
                .Where(cm => cm.UserId == userId && !cm.IsDeleted)
                .Select(cm => cm.ConversationId)
                .ToListAsync();
            if (listId.Any())
            {
                return DbSet.AsQueryable()
                    .Where(c => listId.Contains(c.Guid) && !c.IsDeleted);
            }
            else
            {
                return DbSet.AsQueryable().Where(c => false); // No conversations found for the user
            }
        }

        public async Task<Conversation?> GetPrivateConversationBetweenUsersAsync(Guid userId1, Guid userId2)
        {
            var ids = new List<Guid>(){userId1, userId2};
            var conversationId = await _context.ConversationMembers
                .Where(cm => cm.UserId.HasValue && ids.Contains(cm.UserId.Value) && !cm.IsDeleted)
                .GroupBy(cm => cm.ConversationId)
                .Where(g => g.Count() == 2)
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
            if (conversationId == null || conversationId == Guid.Empty) return null;
            var conversation = await GetByIdAsync(conversationId.Value);
            return conversation;
        }

        public Task<bool> IsUserInThisConversation(Guid conversationId, Guid userId)
        {
            return _context.ConversationMembers
                .AnyAsync(cm => cm.ConversationId == conversationId && cm.UserId == userId && !cm.IsDeleted);
        }

        public async Task<bool> CreateGroupConversationsAsync(Conversation conversation, List<ConversationMember> members, Guid createdBy)
        {
            if (conversation == null) throw new ArgumentNullException(nameof(conversation), "Conversation cannot be null.");
            if (members == null || !members.Any()) throw new ArgumentNullException(nameof(members), "Members cannot be null or empty.");
            conversation.CreatedAt = DateTime.UtcNow;
            conversation.UpdatedAt = DateTime.UtcNow;
            conversation.IsDeleted = false;
            // Check if members contain the creator of the conversation
            var creatorExists = members.Any(m => m.UserId == createdBy);
            if (!creatorExists)
            {
                // If not, add the creator as a member
                members.Add(new ConversationMember
                {
                    ConversationId = conversation.Guid,
                    UserId = createdBy,
                    Role = UserRole.Creator, 
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    DeletedAt = null,
                    IsDeleted = false,
                    Nickname = "", 
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                // Update the role of the creator to Creator if they are already a member
                var creatorMember = members.First(m => m.UserId == createdBy);
                creatorMember.Role = UserRole.Creator;
                creatorMember.UpdatedAt = DateTime.UtcNow;
            }
            await _context.Conversations.AddAsync(conversation);
            await _context.ConversationMembers.AddRangeAsync(members);
            return true;
        }

        public async Task<IQueryable<ConversationEvent>> GetConversationEventsAsync(Guid conversationId)
        {
            return _context.ConversationEvents
                .Where(ce => ce.ConversationId == conversationId && !ce.IsDeleted)
                .AsQueryable();
        }
            
        public Task<IQueryable<Attachment>> GetConversationAttachmentsAsync(Guid conversationId)
        {
            return _context.Attachments
                .Where(a => a.ConversationId == conversationId && !a.IsDeleted)
                .AsQueryable()
                .ToListAsync().ContinueWith(t => t.Result.AsQueryable());
        }

        public Task<ConversationSetting?> GetConversationSettingByUserId(Guid conversationId, Guid userId)
        {
            return _context.ConversationSettings
                .FirstOrDefaultAsync(cs => cs.ConversationId == conversationId && cs.UserId == userId && !cs.IsDeleted);
        }

        public async Task<ConversationSetting> CreateDefaultConversationSettingByUserId(Guid conversationId, Guid userId)
        {
            var existSetting = await _context.ConversationSettings
                .FirstOrDefaultAsync(cs => cs.ConversationId == conversationId && cs.UserId == userId && !cs.IsDeleted);
            if (existSetting != null)
            {
                throw new DuplicateException(
                    $"Conversation setting with user {userId} for conversation {conversationId} is exists");
            }
            // Create new setting with default values
            var setting = new ConversationSetting
            {
                ConversationId = conversationId,
                UserId = userId,
                MuteNotification = false,
                Pinned = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            await _context.ConversationSettings.AddAsync(setting);
            return setting;
        }

        public Task<ConversationStatus?> GetConversationStatusAsync(Guid conversationId, Guid userId)
        {
            return _context.ConversationStatuses
                .FirstOrDefaultAsync(cs => cs.ConversationId == conversationId && cs.UserId == userId && !cs.IsDeleted);
        }

        public async Task<ConversationStatus> CreateDefaultConversationStatusAsync(Guid conversationId, Guid userId)
        {
            var existStatus = await _context.ConversationStatuses
                .FirstOrDefaultAsync(cs => cs.ConversationId == conversationId && cs.UserId == userId && !cs.IsDeleted);
            if (existStatus != null)
            {
                throw new DuplicateException(
                    $"Conversation status with user {userId} for conversation {conversationId} is exists");
            }
            // Create new status with default values
            var status = new ConversationStatus
            {
                ConversationId = conversationId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            await _context.ConversationStatuses.AddAsync(status);
            return status;
        }
    }
}
