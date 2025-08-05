using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class FriendRepository(AppDbContext context) : GenericRepository<Friend>(context), IFriendRepository
    {
        public Task<IQueryable<Friend>> GetIncomingRequestAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Friend>> GetOutgoingRequestAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task SendFriendRequestAsync(Guid userId, Guid targetUserId)
        {
            throw new NotImplementedException();
        }

        public Task AcceptFriendRequestAsync(Guid requesterId, Guid targetUserId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFriendRequestAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeclineFriendRequestAsync(Guid requesterId, Guid targetUserId)
        {
            throw new NotImplementedException();
        }

        public Task BlockFriendAsync(Guid requesterId, Guid targetUserId)
        {
            throw new NotImplementedException();
        }

        public Task<Friend?> GetFriendBetweenUsersAsync(Guid userAId, Guid userBId)
        {
            return _dbSet.FirstOrDefaultAsync(f => (f.RequesterId == userAId && f.AddresseeId == userBId) || (f.RequesterId == userBId && f.AddresseeId == userAId));
        }
    }
}
