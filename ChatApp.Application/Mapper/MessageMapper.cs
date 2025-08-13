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
    public class MessageMapper : IMessageMapper
    {
        public MessageResponseDto MapToResponseDto(Message entity)
        {
            throw new NotImplementedException();
        }
    }
}
