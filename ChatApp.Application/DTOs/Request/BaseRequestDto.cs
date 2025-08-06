namespace ChatApp.Application.DTOs.Request
{
    public abstract class BaseRequestDto
    {
        public Guid? TransactionId { get; set; }
    }
}
