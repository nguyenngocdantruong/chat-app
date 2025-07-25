namespace ChatApp.Domain.Exceptions.Friend
{
    public class BlockedUserException : AppException
    {
        public override int StatusCode => 403; // Forbidden
        public override string ErrorCode => "blocked_user";
        
        public BlockedUserException() : base(403, "You cannot perform this action because the user is blocked.")
        {
        }
        public BlockedUserException(string message) : base(403, message)
        {
        }
    }
}
