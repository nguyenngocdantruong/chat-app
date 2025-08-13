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
    public class FcmTokenMapper : IFcmTokenMapper
    {
        public FcmTokenResponseDto MapToResponseDto(FcmToken entity)
        {
            throw new NotImplementedException();
        }
    }
}
