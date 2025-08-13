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
    public class FcmTokenRepository: GenericRepository<FcmToken>, IFcmTokenRepository
    {
        public FcmTokenRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<FcmToken>> GetListFcmTokensByUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFcmToken(Guid userId, FcmToken fcmToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFcmToken(Guid userId, FcmToken fcmToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveInvalidFcmToken(FcmToken fcmToken)
        {
            throw new NotImplementedException();
        }
    }
}
