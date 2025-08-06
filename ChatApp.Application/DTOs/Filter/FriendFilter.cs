using System.ComponentModel.DataAnnotations;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Filter
{
    public class FriendFilter: BaseFilter
    {
        [EnumDataType(typeof(FriendStatus))]
        public FriendStatus? Status { get; set; }
    }
}
