using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Security;

namespace ChatApp.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork) : GenericService<User>(unitOfWork), IUserService
    {
        public async Task<User> GetCurrentUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            User? user = await unitOfWork.UserRepository.GetByUID(userId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {userId} not found.");
            }
            return user;
        }

        public async Task<bool> ComparePasswordAsync(Guid userId, string password)
        {
            var user = await unitOfWork.UserRepository.GetByUID(userId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {userId} not found.");
            }

            if (string.IsNullOrEmpty(password)) return false;
            return PasswordHasher.Verify(password, user.PasswordHash);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }
            var user = await unitOfWork.UserRepository.GetByEmailAsync(email);
            return user;
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
            var user = await unitOfWork.UserRepository.GetByUID(uid);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {uid} not found.");
            }
            user.IsDeleted = true;
            await unitOfWork.SaveChangesAsync();
        }
    }
}
