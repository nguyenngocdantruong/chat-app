using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Interfaces;

namespace ChatApp.Application.Services
{
    public class AuditLogService(IUnitOfWork uow): IAuditLogService
    {
        public async Task SaveLogAsync(string action, Guid userId, Guid targetId, string? note)
        {
            await uow.AuditLogRepository.SaveLogAsync(new Domain.Entities.AuditLog
            {
                UserId = userId,
                TargetId = targetId,
                Action = action,
                Note = note
            });
        }
    }
}
