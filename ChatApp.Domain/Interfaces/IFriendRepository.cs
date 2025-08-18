using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IFriendRepository: IGenericRepository<Friend>
    {
        Task<IQueryable<Friend>> GetIncomingRequestAsync(Guid userId);
        Task<IQueryable<Friend>> GetOutgoingRequestAsync(Guid userId);

        //Gửi lời mời kb
        Task SendFriendRequestAsync(Guid userId, Guid targetUserId);
        //Chấp nhận lời mời kb
        Task AcceptFriendRequestAsync(Guid requesterId, Guid addressUserId);
        // Huỷ kết bạn
        Task RemoveFriendRequestAsync(Guid userId);
        // Từ chối lời mời kb
        Task DeclineFriendRequestAsync(Guid requesterId, Guid targetUserId);
        // Chặn
        Task BlockFriendAsync(Guid requesterId, Guid targetUserId);
        // Lấy ra tình trạng
        Task<Friend?> GetFriendBetweenUsersAsync(Guid userAId, Guid userBId);
    }
}
