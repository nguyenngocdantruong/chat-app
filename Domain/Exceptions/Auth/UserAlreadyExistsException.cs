namespace ChatApp.Domain.Exceptions.Auth
{
    public class UserAlreadyExistsException: AppException
    {
        public override int StatusCode => 409; // Conflict
        public override string ErrorCode => "user_already_exists";
        public UserAlreadyExistsException() : base(409, "User already exists.")
        {
        }
        public UserAlreadyExistsException(string message) : base(409, message)
        {
        }
    }
}
