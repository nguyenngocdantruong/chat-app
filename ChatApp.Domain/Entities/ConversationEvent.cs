using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

public class ConversationEvent : BaseEntity
{
    public ConversationEventType Type { get; set; } 
    public Guid ConversationId { get; set; }              
    public Guid? UserId { get; set; }                     
    public Guid? TargetUserId { get; set; }              
    public Guid? MessageId { get; set; }                  

    public string? OldValue { get; set; }                 
    public string? NewValue { get; set; }                 
}
