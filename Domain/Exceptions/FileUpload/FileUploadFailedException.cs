namespace ChatApp.Domain.Exceptions.FileUpload
{
    public class FileUploadFailedException: AppException
    {
        public override int StatusCode => 500; // Internal Server Error
        public override string ErrorCode => "file_upload_failed";
        public FileUploadFailedException() : base(500, "Failed to upload the file.")
        {
        }
        public FileUploadFailedException(string message) : base(500, message)
        {
        }
    }
}
