using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Response;

public class ConversationEventResponseDto : BaseResponseDto
{
    public Guid Id { get; set; }
    public ConversationEventType Type { get; set; } 
    public Guid ConversationId { get; set; }              
    public Guid? UserId { get; set; }                     
    public Guid? TargetId { get; set; }              

    public object? OldValue { get; set; }                 
    public object? NewValue { get; set; }                 
}