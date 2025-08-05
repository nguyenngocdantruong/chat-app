namespace ChatApp.Application.DTOs.Filter
{
    public class ConversationFilter : BaseFilter
    {
        public bool? IsGroup { get; set; }
        public string? MemberName { get; set; }
    }
}