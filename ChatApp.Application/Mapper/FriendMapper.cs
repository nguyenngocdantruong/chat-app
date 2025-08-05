using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Mapper
{
    public class FriendMapper : IFriendMapper
    {
        public FriendResponseDto MapToResponseDto(Friend entity)
        {
            return new FriendResponseDto()
            {
                AddresseeId = entity.AddresseeId,
                RequesterId = entity.RequesterId,
                Status = entity.Status,
            };
        }
    }
}
