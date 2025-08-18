using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class FriendRepository(AppDbContext context) : GenericRepository<Friend>(context), IFriendRepository
    {
        public Task<IQueryable<Friend>> GetIncomingRequestAsync(Guid userId)
        {
            return Task.FromResult(DbSet.Where(f => f.AddresseeId == userId && f.Status == FriendStatus.Pending).AsQueryable());
        }

        public Task<IQueryable<Friend>> GetOutgoingRequestAsync(Guid userId)
        {
            return Task.FromResult(DbSet.Where(f => f.RequesterId == userId && f.Status == FriendStatus.Pending).AsQueryable());
        }

        public Task SendFriendRequestAsync(Guid userId, Guid targetUserId)
        {
            var friendRequest = new Friend
            {
                RequesterId = userId,
                AddresseeId = targetUserId,
                Status = FriendStatus.Pending
            };
            return AddAsync(friendRequest);
        }

        public async Task AcceptFriendRequestAsync(Guid requesterId, Guid addressUserId)
        {
            var current = await DbSet.FirstOrDefaultAsync(m =>
                m.RequesterId == requesterId && m.AddresseeId == addressUserId && m.Status == FriendStatus.Pending);
            if (current != null)
            {
                current.Status = FriendStatus.Accepted;
                await Update(current);
            }
            else
            {
                throw new BadRequestException("Friend request not found or already accepted.");
            }
        }

        public async Task RemoveFriendRequestAsync(Guid userId)
        {
            var friendRequest = await DbSet.FirstOrDefaultAsync(f => f.RequesterId == userId || f.AddresseeId == userId);
            if (friendRequest == null)
            {
                throw new BadRequestException("Friend request not found.");
            }
            DbSet.Remove(friendRequest);
        }

        public async Task DeclineFriendRequestAsync(Guid requesterId, Guid targetUserId)
        {
            var friendRequest = await DbSet.FirstOrDefaultAsync(f => f.RequesterId == requesterId && f.AddresseeId == targetUserId && f.Status == FriendStatus.Pending);
            if (friendRequest == null)
            {
                throw new BadRequestException("Friend request not found.");
            }
            await Delete(friendRequest);
        }

        public async Task BlockFriendAsync(Guid requesterId, Guid targetUserId)
        {
            var existingFriend = await GetFriendBetweenUsersAsync(requesterId, targetUserId);
            
            if (existingFriend != null)
            {
                existingFriend.Status = FriendStatus.Blocked;
                await Update(existingFriend);
            }
            else
            {
                var friendRequest = new Friend
                {
                    RequesterId = requesterId,
                    AddresseeId = targetUserId,
                    Status = FriendStatus.Blocked
                };
                await AddAsync(friendRequest);
            }
        }

        public Task<Friend?> GetFriendBetweenUsersAsync(Guid userAId, Guid userBId)
        {
            return DbSet.FirstOrDefaultAsync(f => (f.RequesterId == userAId && f.AddresseeId == userBId) || (f.RequesterId == userBId && f.AddresseeId == userAId));
        }
    }
}
