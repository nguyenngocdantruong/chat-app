namespace ChatApp.Domain.Exceptions.FileUpload
{
    public class FileTooLargeException: AppException
    {
        public override int StatusCode => 413; // Payload Too Large
        public override string ErrorCode => "file_too_large";
        public FileTooLargeException() : base(413, "The uploaded file is too large.")
        {
        }
        public FileTooLargeException(string message) : base(413, message)
        {
        }
    }
}
