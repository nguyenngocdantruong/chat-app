using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class ConversationMemberRepository(AppDbContext context)
        : GenericRepository<ConversationMember>(context), IConversationMemberRepository
    {
        public async Task<IEnumerable<ConversationMember>> GetMembersAsync(Guid conversationId)
        {
            var conversation = GetByIdAsync(conversationId);
            if (conversation == null)
            {
                throw new ArgumentNullException($"Conversation with ID {conversationId} does not exist.", nameof(conversationId));
            }

            return await DbSet.AsQueryable()
                .Where(cm => cm.ConversationId == conversationId).ToListAsync();
        }

        public async Task AddMemberAsync(Guid conversationId, Guid userId)
        {
            var conversation = await context.Conversations.FirstOrDefaultAsync(m => m.Guid == conversationId);
            if (conversation == null)
            {
                throw new ArgumentNullException($"Conversation with ID {conversationId} does not exist.", nameof(conversationId));
            }
            var user = await context.Users.FirstOrDefaultAsync(m => m.Guid == userId);
            if (user == null)
            {
                throw new ArgumentNullException($"User with ID {userId} does not exist.", nameof(userId));
            }
            ConversationMember member = new ConversationMember
            {
                ConversationId = conversationId,
                UserId = userId,
                Role = UserRole.Member, 
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                DeletedAt = null,
                IsDeleted = false,
                Nickname = "",
                UpdatedAt = DateTime.UtcNow
            };
            await DbSet.AddAsync(member);
        }

        public async Task<bool> UpdateMemberRoleAsync(Guid conversationId, Guid userId, UserRole role)
        {
            var conversation = await context.Conversations.FirstOrDefaultAsync(m => m.Guid == conversationId);
            if (conversation == null)
            {
                throw new ArgumentNullException($"Conversation with ID {conversationId} does not exist.", nameof(conversationId));
            }
            var member = await DbSet.FirstOrDefaultAsync(m => m.ConversationId == conversationId && m.UserId == userId);
            if (member == null)
            {
                throw new ArgumentNullException($"Member with User ID {userId} does not exist in conversation {conversationId}.", nameof(userId));
            }
            member.Role = role;
            member.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public async Task RemoveMemberAsync(Guid conversationId, Guid userId)
        {
            var conversation = await context.Conversations.FirstOrDefaultAsync(m => m.Guid == conversationId);
            if (conversation == null)
            {
                throw new ArgumentNullException($"Conversation with ID {conversationId} does not exist.", nameof(conversationId));
            }
            var member = await DbSet.FirstOrDefaultAsync(m => m.ConversationId == conversationId && m.UserId == userId);
            if (member == null)
            {
                throw new ArgumentNullException($"Member with User ID {userId} does not exist in conversation {conversationId}.", nameof(userId));
            }
            member.IsDeleted = true;
            member.DeletedAt = DateTime.UtcNow;
        }
    }
}
