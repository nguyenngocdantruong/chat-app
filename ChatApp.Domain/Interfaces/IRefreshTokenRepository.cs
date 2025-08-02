using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IRefreshTokenRepository: IGenericRepository<RefreshToken>
    {
        Task<IEnumerable<RefreshToken>> GetAllTokenActiveByUserIdAsync(Guid userId);
        Task RevokeAllTokenByUserIdAsync(Guid userId);

    }
}
