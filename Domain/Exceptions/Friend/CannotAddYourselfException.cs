namespace ChatApp.Domain.Exceptions.Friend
{
    public class CannotAddYourselfException : AppException
    {
        public override int StatusCode => 400; // Bad request
        public override string ErrorCode => "cannot_add_yourself";
        public CannotAddYourselfException() : base(400, "You cannot add yourself as a friend.")
        {
        }
        public CannotAddYourselfException(string message) : base(400, message)
        {
        }
    }
}
