namespace ChatApp.Domain.Exceptions.FileUpload
{
    public class StorageLimitExceededException: AppException
    {
        public override int StatusCode => 507; // Insufficient Storage
        public override string ErrorCode => "storage_limit_exceeded";
        
        public StorageLimitExceededException() : base(507, "Storage limit exceeded.")
        {
        }
        public StorageLimitExceededException(string message) : base(507, message)
        {
        }
    }
}
