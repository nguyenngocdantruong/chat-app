namespace ChatApp.Domain.Exceptions.Auth
{
    public class UnAuthorizedAccessException: AppException
    {
        public override int StatusCode => 401; // Unauthorized
        public override string ErrorCode => "unauthorized_access";
        
        public UnAuthorizedAccessException() : base(401, "Unauthorized access.")
        {
        }
        
        public UnAuthorizedAccessException(string message) : base(401, message)
        {
        }
    }
}
