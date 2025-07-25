namespace ChatApp.Domain.Exceptions.Auth
{
    public sealed class InvalidCredentialsException : AppException
    {
        public override int StatusCode => 401; // Unauthorized
        public override string ErrorCode => "invalid_credential";
        public InvalidCredentialsException() : base(401, "Invalid credentials provided.")
        {
        }
        public InvalidCredentialsException(string message) : base(401, message)
        {
        }
    }
}
