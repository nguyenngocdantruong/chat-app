using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class AuditLogRepository(AppDbContext context) : GenericRepository<AuditLog>(context), IAuditLogRepository
    {
        public async Task SaveLogAsync(AuditLog log)
        {
            if (log == null) throw new BadRequestException($"Log null {nameof(log.Data)}");
            await _dbSet.AddAsync(log);
        }
    }
}
