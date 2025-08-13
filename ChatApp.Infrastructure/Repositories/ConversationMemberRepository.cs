using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;

namespace ChatApp.Infrastructure.Repositories
{
    public class ConversationMemberRepository(AppDbContext context)
        : GenericRepository<ConversationMember>(context), IConversationMemberRepository
    {
        public Task<IEnumerable<User>> GetMembersAsync(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public Task AddMemberAsync(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberRoleAsync(Guid conversationId, Guid userId, UserRole role)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMemberAsync(Guid conversationId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
