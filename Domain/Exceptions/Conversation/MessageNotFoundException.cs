namespace ChatApp.Domain.Exceptions.Conversation
{
    public class MessageNotFoundException: AppException
    {
        public override int StatusCode => 404; // Not found
        public override string ErrorCode => "message_not_found";
        
        public MessageNotFoundException() : base(404, "Message not found.")
        {
        }
        public MessageNotFoundException(string message) : base(404, message)
        {
        }
    }
}
