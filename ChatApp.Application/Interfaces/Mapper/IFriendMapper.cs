using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Mapper
{
    public interface IFriendMapper : IDtoMapper<Friend, FriendResponseDto>
    {

    }
}
