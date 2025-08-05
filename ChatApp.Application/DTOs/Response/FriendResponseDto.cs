using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Response;

public partial class FriendResponseDto
{
    public Guid? RequesterId { get; set; }

    public Guid? AddresseeId { get; set; }

    public FriendStatus Status { get; set; }
}

