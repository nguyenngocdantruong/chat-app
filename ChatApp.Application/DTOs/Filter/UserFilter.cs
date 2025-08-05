namespace ChatApp.Application.DTOs.Filter
{
    public class UserFilter : BaseFilter
    {
        public bool? IsOnline { get; set; }
        public bool? IsActive { get; set; }
    }
}
