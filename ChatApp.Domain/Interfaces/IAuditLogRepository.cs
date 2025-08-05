using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IAuditLogRepository
    {
        Task SaveLogAsync(AuditLog log);
    }
}
