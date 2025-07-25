namespace ChatApp.Domain.Exceptions.Conversation
{
    public class MessageSendFailedException: AppException
    {
        public override int StatusCode => 500; // Internal Server Error
        public override string ErrorCode => "message_send_failed";
        public MessageSendFailedException() : base(500, "Failed to send the message.")
        {
        }
        public MessageSendFailedException(string message) : base(500, message)
        {
        }
    }
}
