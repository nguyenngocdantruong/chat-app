namespace ChatApp.Domain.Exceptions.Conversation
{
    public class UserNotInConversationException : AppException
    {
        public override int StatusCode => 403; // Forbidden
        public override string ErrorCode => "user_not_in_conversation";

        public UserNotInConversationException() : base(403, "User is not part of the conversation.")
        {
        }

        public UserNotInConversationException(string message) : base(403, message)
        {
        }
    }
}
