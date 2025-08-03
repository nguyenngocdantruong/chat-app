using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Security;

namespace ChatApp.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IUserMapper mapper)
        : GenericService<User, UserResponseDto>(unitOfWork, mapper), IUserService
    {
        public async Task<User> GetCurrentUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            User? user = await UnitOfWork.UserRepository.GetByUID(userId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {userId} not found.");
            }
            return user;
        }

        public async Task<UserResponseDto?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            var user = await UnitOfWork.UserRepository.GetByEmailAsync(email);
            return user == null ? null : mapper.MapToResponseDto(user);
        }

        Task<UserResponseDto?> IUserService.GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        Task<FcmTokenResponseDto> IUserService.RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        Task<FcmTokenResponseDto> IUserService.UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ComparePasswordAsync(Guid userId, string password)
        {
            var user = await UnitOfWork.UserRepository.GetByUID(userId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {userId} not found.");
            }

            if (string.IsNullOrEmpty(password)) return false;
            return PasswordHasher.Verify(password, user.PasswordHash);
        }

        public Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto changePasswordRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task ResetPasswordAsync(Guid userId, ResetPasswordRequestDto resetPasswordRequestDto)
        {
            throw new NotImplementedException();
        }

        Task<UserResponseDto> IUserService.GetCurrentUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByUsername(string username)
        {
            return await UnitOfWork.UserRepository.GetByUsernameAsync(username);
        }

        public Task<FcmToken> RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<FcmToken> UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAccountAsync(Guid currentUserId, Guid uid)
        {
            if (currentUserId != uid)
            {
                throw new ValidationException("You can only delete your own account.");
            }
            var user = await UnitOfWork.UserRepository.GetByUID(uid);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {uid} not found.");
            }
            user.IsDeleted = true;
            await UnitOfWork.SaveChangesAsync();
        }
    }
}
