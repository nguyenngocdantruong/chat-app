namespace ChatApp.Domain.Exceptions.FileUpload
{
    public class InvalidFileTypeException: AppException
    {
        public override int StatusCode => 415; // Unsupported Media Type
        public override string ErrorCode => "invalid_file_type";
        public InvalidFileTypeException() : base(415, "The uploaded file type is not supported.")
        {
        }
        public InvalidFileTypeException(string message) : base(415, message)
        {
        }
    }
}
