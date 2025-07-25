using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

public class ConversationEvent : BaseEntity
{
    public ConverstationEventType Type { get; set; } 
    public Guid ConversationId { get; set; }              
    public Guid? UserId { get; set; }                     
    public Guid? TargetUserId { get; set; }              
    public Guid? MessageId { get; set; }                  

    public string? OldValue { get; set; }                 
    public string? NewValue { get; set; }                 

    [ForeignKey("ConversationId")]
    public virtual Conversation? Conversation { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    [ForeignKey("TargetUserId")]
    public virtual User? TargetUser { get; set; }

    [ForeignKey("MessageId")]
    public virtual Message? Message { get; set; }
}
