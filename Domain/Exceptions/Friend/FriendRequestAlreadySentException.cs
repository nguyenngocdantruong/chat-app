namespace ChatApp.Domain.Exceptions.Friend
{
    public class FriendRequestAlreadySentException: AppException
    {
        public override int StatusCode => 409; // Conflict
        public override string ErrorCode => "friend_request_already_sent";
        public FriendRequestAlreadySentException() : base(409, "Friend request already sent.")
        {
        }
        public FriendRequestAlreadySentException(string message) : base(409, message)
        {
        }
    }
}
