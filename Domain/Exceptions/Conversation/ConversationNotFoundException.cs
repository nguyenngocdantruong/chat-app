namespace ChatApp.Domain.Exceptions.Conversation
{
    public class ConversationNotFoundException : AppException
    {
        public override int StatusCode => 404; // Not found
        public override string ErrorCode => "conversation_not_found";
        public ConversationNotFoundException() : base(404, "Conversation not found.")
        {
        }
        public ConversationNotFoundException(string message) : base(404, message)
        {
        }
    }
}
