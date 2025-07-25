namespace ChatApp.Domain.Exceptions.Conversation
{
    public class MessageTypeNotSupportedException : AppException
    {
        public override int StatusCode => 400; // Bad Request
        public override string ErrorCode => "message_type_not_supported";
        public MessageTypeNotSupportedException() : base(400, "The message type is not supported.")
        {
        }
        public MessageTypeNotSupportedException(string message) : base(400, message)
        {
        }
    }
}
