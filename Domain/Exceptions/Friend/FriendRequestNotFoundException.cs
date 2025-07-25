namespace ChatApp.Domain.Exceptions.Friend
{
    public class FriendRequestNotFoundException: AppException
    {
        public override int StatusCode => 404; // Not found
        public override string ErrorCode => "friend_request_not_found";
        public FriendRequestNotFoundException() : base(409, "Friend request not found.")
        {
        }
        public FriendRequestNotFoundException(string message) : base(409, message)
        {
        }
    }
}
