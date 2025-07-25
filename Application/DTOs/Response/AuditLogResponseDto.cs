using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
namespace ChatApp.Application.DTOs.Response
{
    public class AuditLog: BaseEntity
    {
        public Guid UserId { get; set; }
        public ActionType Action { get; set; }
        public string Data { get; set; } = null!; // Json string representation of the action data
    }
}
