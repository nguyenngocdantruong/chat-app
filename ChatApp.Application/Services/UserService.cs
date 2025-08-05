using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Common;
using ChatApp.Shared.Security;

namespace ChatApp.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IUserRepository repository, 
        IUserMapper mapper, ICurrentUserService currentUserService,
        IFriendService friendService)
        : GenericService<User, UserResponseDto>(unitOfWork, repository, mapper), IUserService
    {
        private async Task<bool> CanViewProfile(Guid targetId)
        {
            var currentUserId = currentUserService.UserId;
            var user = await repository.GetByIdAsync(targetId);
            if(user == null)
            {
                throw new RecordNotFoundException($"User with ID {targetId} not found.");
            }
            bool isSelfRequest = currentUserId.HasValue && user.Guid == currentUserId.Value;
            FriendResponseDto friendResponse =
                (await friendService.GetFriendBetweenUsers(user.Guid, currentUserId.Value)).Data;
            bool isValidToViewDetail = (user.IsSearchable.HasValue && user.IsSearchable.Value) || (friendResponse is { Status: FriendStatus.Accepted });
            return isSelfRequest || isValidToViewDetail;
        }
        public async Task<Result<UserResponseDto>> GetCurrentUser()
        {
            var userId = currentUserService.UserId;
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            var user = await GetByIdAsync(userId.Value);
            var result = new Result<UserResponseDto>()
            {
                Data = user,
                IsSuccess = user != null,
                Message = ""
            };
            return result;
        }

        public async Task<Result<UserResponseDto>> GetByEmailAsync(string email, bool isFromAuthAction = true)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            var user = await repository.GetByEmailAsync(email);
            //Check if invalid request to view user details
            if (user == null)
            {
                return new Result<UserResponseDto>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Data = user == null ? null : mapper.MapToResponseDto(user)
                };
            }

            bool canView = isFromAuthAction || await CanViewProfile(user.Guid);
            return new Result<UserResponseDto>
            {
                IsSuccess = canView,
                Message = canView ? "User retrieved successfully" : "You cannot view this user right now",
                Data = canView ? mapper.MapToResponseDto(user) : null
            };
        }

        public async Task<Result<UserResponseDto>> GetByUsername(string username, bool isFromAuthAction = true)
        {
            var user = await repository.GetByUsernameAsync(username);
            bool canView = user != null && (isFromAuthAction || await CanViewProfile(user.Guid));
            Result<UserResponseDto> result = new Result<UserResponseDto>()
            {
                IsSuccess = canView,
                Message = canView ? "User retrieved successfully" : "You cannot view this user right now",
                Data = user != null && canView ? mapper.MapToResponseDto(user) : null
            };
            return result;
        }

        Task<Result<FcmTokenResponseDto>> IUserService.RegisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        Task<Result<FcmTokenResponseDto>> IUserService.UnregisterFcmTokenAsync(Guid userId, FcmTokenRequestDto fcmTokenRequestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ComparePasswordAsync(Guid userId, string password)
        {
            var user = await repository.GetByUID(userId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {userId} not found.");
            }

            if (string.IsNullOrEmpty(password)) return false;
            return PasswordHasher.Verify(password, user.PasswordHash);
        }

        public async Task<Result<object>> DeleteAccountAsync()
        {
            var uid = currentUserService.UserId.Value;
            var user = await repository.GetByUID(uid);
            if (user == null)
            {
                throw new RecordNotFoundException($"User with ID {uid} not found.");
            }
            user.IsDeleted = true;
            await UnitOfWork.SaveChangesAsync();
            return new Result<object>
            {
                IsSuccess = true,
                Message = "Account deleted successfully",
                Data = null
            };
        }
    }
}
