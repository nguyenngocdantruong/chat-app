using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Mapper
{
    public interface IUserMapper : IDtoMapper<User, UserResponseDto>
    {
        User Map(RegisterRequestDto requestDto, AttachmentResponseDto? attachmentDto);
        User Map(ChangePasswordRequestDto requestDto, User user);
        User Map(ResetPasswordRequestDto requestDto, User user);
        User Map(UpdateProfileRequestDto requestDto, AttachmentResponseDto? attachmentDto, User user);
    }
}
