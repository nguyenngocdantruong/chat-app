namespace ChatApp.Domain.Exceptions.Auth
{
    public class PerrmissionDeniedException: AppException
    {
        public override int StatusCode => 403; // Forbidden
        public override string ErrorCode => "permission_denied";
        public PerrmissionDeniedException() : base(403, "Permission denied.")
        {
        }
        public PerrmissionDeniedException(string message) : base(403, message)
        {
        }
    }
}
