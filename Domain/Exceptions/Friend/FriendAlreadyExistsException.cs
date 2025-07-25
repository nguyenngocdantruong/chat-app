namespace ChatApp.Domain.Exceptions.Friend
{
    public class FriendAlreadyExistsException: AppException
    {
        public override int StatusCode => 409; // Conflict
        public override string ErrorCode => "friend_already_exists";
        public FriendAlreadyExistsException() : base(409, "Friend already exists.")
        {
        }
        public FriendAlreadyExistsException(string message) : base(409, message)
        {
        }
    }
}
