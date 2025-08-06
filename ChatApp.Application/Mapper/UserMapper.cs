using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Domain.Entities;
using ChatApp.Shared.Security;

namespace ChatApp.Application.Mapper
{
    public class UserMapper : IUserMapper
    {

        public User Map(RegisterRequestDto requestDto, AttachmentResponseDto? attachmentDto)
        {
            return new User
            {
                Username = requestDto.Username,
                Email = requestDto.Email,
                PasswordHash = PasswordHasher.Hash(requestDto.Password),
                DisplayName = requestDto.DisplayName,
                AvatarUrl = attachmentDto?.FileUrl
            };
        }

        public User Map(ChangePasswordRequestDto requestDto, User user)
        {
            user.PasswordHash = PasswordHasher.Hash(requestDto.NewPassword);
            return user;
        }

        public User Map(UpdateProfileRequestDto requestDto, AttachmentResponseDto? attachmentDto, User user)
        {
            user.Phone = requestDto.Phone;
            if(!string.IsNullOrEmpty(requestDto.NewPassword))
                user.PasswordHash = PasswordHasher.Hash(requestDto.NewPassword);
            user.AvatarUrl = attachmentDto?.FileUrl;
            user.DisplayName = requestDto.DisplayName;
            user.IsSearchable = requestDto.IsSearchable ?? true;
            return user;
        }

        public FcmTokenResponseDto MapToFcmTokenResponseDto(FcmToken token)
        {
            throw new NotImplementedException();
        }

        public FcmToken MapToFcmToken(FcmTokenRequestDto requestDto, FcmToken token)
        {
            throw new NotImplementedException();
        }

        public User Map(ResetPasswordRequestDto requestDto, User user)
        {
            user.PasswordHash = PasswordHasher.Hash(requestDto.NewPassword);
            return user;
        }

        public UserResponseDto MapToResponseDto(User entity)
        {
            return new UserResponseDto
            {
                Guid = entity.Guid,
                Username = entity.Username,
                Email = entity.Email,
                Phone = entity.Phone,
                DisplayName = entity.DisplayName,
                IsSearchable = entity.IsSearchable ?? true,
                LastSeen = entity.LastSeen,
                IsOnline = entity.IsOnline
            };
        }
    }
}
