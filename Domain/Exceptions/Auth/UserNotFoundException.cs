namespace ChatApp.Domain.Exceptions.Auth
{
    public class UserNotFoundException: AppException
    {
        public override int StatusCode => 404; // Not Found
        public override string ErrorCode => "user_not_found";
        
        public UserNotFoundException() : base(404, "User not found.")
        {
        }
        
        public UserNotFoundException(string message) : base(404, message)
        {
        }
    }
}
