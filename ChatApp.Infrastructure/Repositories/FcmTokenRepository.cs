using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class FcmTokenRepository: GenericRepository<FcmToken>, IFcmTokenRepository
    {
        public FcmTokenRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<FcmToken>> GetListFcmTokensByUserAsync(Guid userId)
        {
            return DbSet.Where(m => m.UserId == userId && !m.IsRevoked && !m.IsDeleted).ToListAsync();
        }

        public async Task SaveFcmToken(Guid userId, FcmToken fcmToken)
        {
            if(fcmToken == null)
            {
                throw new ArgumentNullException(nameof(fcmToken), "FcmToken cannot be null");
            }
            if (fcmToken.UserId != userId)
            {
                throw new ArgumentException("FcmToken does not belong to the specified user", nameof(userId));
            }
            await DbSet.AddAsync(fcmToken);
        }

        public async Task RemoveFcmToken(Guid userId, FcmToken fcmToken)
        {
            var fcmTokenInDb = await DbSet.FirstOrDefaultAsync(m => m.Guid == fcmToken.Guid && m.UserId == userId && !m.IsDeleted);
            if (fcmTokenInDb == null)
            {
                throw new RecordNotFoundException($"FcmToken {fcmToken.Guid} not found for the specified user {userId}");
            }
            fcmTokenInDb.IsDeleted = true;
        }

        public async Task RemoveInvalidFcmToken(FcmToken fcmToken)
        {
            var invalidToken = await DbSet.FirstOrDefaultAsync(m => m.Guid == fcmToken.Guid && !m.IsDeleted);
            if (invalidToken == null)
            {
                throw new RecordNotFoundException($"FcmToken {fcmToken.Guid} not found or already deleted");
            }
            invalidToken.IsRevoked = true;
            invalidToken.IsDeleted = true;
        }
    }
}
