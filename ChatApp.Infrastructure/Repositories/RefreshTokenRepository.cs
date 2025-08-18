using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context)
        : GenericRepository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<IEnumerable<RefreshToken>> GetAllTokenActiveByUserIdAsync(Guid userId)
        {
            return await DbSet.Where(m => m.UserId == userId && m.IsDeleted == false).ToListAsync();
        }

        public async Task RevokeAllTokenByUserIdAsync(Guid userId)
        {
            var result = await DbSet.Where(m => m.UserId == userId && m.IsDeleted == false).ToListAsync();
            foreach (var refreshToken in result)
            {
                refreshToken.IsRevoked = true;
            }
        }
    }
}
