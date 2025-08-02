using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces;
using ChatApp.Application.Interfaces.Authentication;
using ChatApp.Application.Interfaces.ExternalService;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enums;
using ChatApp.Domain.Exceptions.Authentication;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Exceptions.Validate;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Configurations;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Security;
using ChatApp.Shared.Services;
using ChatApp.Shared.Utils;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ArgNullException = ChatApp.Domain.Exceptions.Runtime.ArgumentNullException;

namespace ChatApp.Application.Services
{
    public class AuthService
        (IUserService userService, IUserMapper mapper, IUnitOfWork unitOfWork,
            ICacheService<string> cacheService, IMailService mailService, ITokenService tokenService,
            IFileService fileService, ITokenSetting tokenSetting, IRefreshTokenService refreshTokenService,
            ICurrentUserService currentUserService)
        : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgNullException(nameof(unitOfWork));
        private readonly IUserService _userService = userService ?? throw new ArgNullException(nameof(userService));
        private readonly IUserMapper _mapper = mapper ?? throw new ArgNullException(nameof(mapper));
        private readonly ICacheService<string> _cacheService = cacheService ?? throw new ArgNullException(nameof(cacheService));
        private readonly IMailService _mailService = mailService ?? throw new ArgNullException(nameof(mailService));
        private readonly ITokenService _tokenService = tokenService ?? throw new ArgNullException(nameof(tokenService));
        private readonly ITokenSetting _tokenSetting = tokenSetting ?? throw new ArgNullException(nameof(tokenSetting));
        private readonly IFileService _fileService = fileService ?? throw new ArgNullException(nameof(fileService));
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService ?? throw new ArgNullException(nameof(refreshTokenService));
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgNullException(nameof(currentUserService));

        public async Task<LoginResponseDto> LoginFirstStep(LoginRequestDto loginRequestDto)
        {

            // Validate the login request
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || string.IsNullOrWhiteSpace(loginRequestDto.Password))
            {
                throw new ValidationException("Email and password must not be empty.");
            }
            LoginResponseDto response = new LoginResponseDto();
            var user = await _userService.GetByEmailAsync(loginRequestDto.Email);
            if(user == null)
            {
                response.IsStepSuccess = false;
                response.User = null;
                response.Token = null;
                return response;
            }

            //Validate the user password
            if (user.Email.Equals(loginRequestDto.Email) &&
                await _userService.ComparePasswordAsync(user.Guid, loginRequestDto.Password))
            {
                //Send email
                string otpRandom = ItemGenerator.GenerateOtp(length: 6);
                await _mailService.SendOtp(user.Email, otpRandom, ActionType.Login);
                // Generate a transaction ID for the first step of login
                string transactionId = ItemGenerator.GenerateRandom();
                _cacheService.Set(transactionId, otpRandom, TimeSpan.FromMinutes(15));
                response.TransactionId = transactionId;
                response.IsStepSuccess = true;
            }
            else
            {
                response.IsStepSuccess = false;
                response.TransactionId = null;
            }
            response.User = null;
            response.Token = null;
            return response;
        }

        public async Task<LoginResponseDto> LoginWith2FaAsync(LoginRequestDto loginRequestDto)
        {
            // Validate the login request
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || string.IsNullOrWhiteSpace(loginRequestDto.Code) || string.IsNullOrEmpty(loginRequestDto.TransactionId))
            {
                throw new ValidationException("Email, code and transaction id must not be empty.");
            }

            LoginResponseDto response = new LoginResponseDto
            {
                IsStepSuccess = false,
                Token = null,
                User = null,
                TransactionId = loginRequestDto.TransactionId
            };

            var user = await _userService.GetByEmailAsync(loginRequestDto.Email);
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Check if the transaction ID is valid
            string otpCodeInCache = _cacheService.Get(loginRequestDto.TransactionId);
            if (otpCodeInCache == null)
            {
                throw new BadRequestException("Invalid transaction ID or expired.");
            }
            // Verify the OTP code
            if (!string.Equals(loginRequestDto.Code, otpCodeInCache, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidCredentialException("Invalid OTP code.");
            }
            // Clear the OTP code from cache after successful verification
            _cacheService.Remove(loginRequestDto.TransactionId);
            // Generate JWT token
            string token = _tokenService.GenerateToken(user.Guid, user.Email ?? "");
            string refreshToken = _tokenService.GenerateRefreshToken();

            // Update the user's refresh token and expiry date
            await _refreshTokenService.CreateAsync(new RefreshToken()
            {
                UserId = user.Guid,
                Token = refreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays),
                IsRevoked = false,
                IsDeleted = false
            });

            var recordSave = await _unitOfWork.SaveChangesAsync();
            if (recordSave <= 0)
            {
                throw new DatabaseOperationException("Failed to save user refresh token.");
            }

            response = new LoginResponseDto
            {
                IsStepSuccess = true,
                Token = new TokenResponseDto()
                {
                    AccessToken = token,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays)
                },
                User = _mapper.MapToResponseDto(user),
                TransactionId = null // Clear transaction ID after successful login
            };
            return response;
        }

        public async Task ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto)
        {
            if (resendEmailRequestDto == null)
            {
                throw new NullValueException("Resend email request cannot be null.");
            }
            // Validate the resend email request
            if (string.IsNullOrWhiteSpace(resendEmailRequestDto.Email) || string.IsNullOrWhiteSpace(resendEmailRequestDto.TransactionId))
            {
                throw new ValidationException("Email and transaction ID must not be empty.");
            }
            var user = await _userService.GetByEmailAsync(resendEmailRequestDto.Email);
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            //Check if the transaction ID is valid
            if (string.IsNullOrWhiteSpace(resendEmailRequestDto.TransactionId) || string.IsNullOrEmpty(_cacheService.Get(resendEmailRequestDto.TransactionId)))
            {
                throw new BadRequestException("Invalid transaction ID or expired.");
            }
            // Send OTP code to the user's email
            string otpRandom = ItemGenerator.GenerateOtp(length: 6);
            await _mailService.SendOtp(user.Email, otpRandom, resendEmailRequestDto.ActionType);
            // Store the OTP code in cache with the transaction ID
            _cacheService.Set(resendEmailRequestDto.TransactionId, otpRandom, TimeSpan.FromMinutes(15));
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto)
        {
            if (userRequestDto == null)
            {
                throw new NullValueException("User request cannot be null.");
            }
            
            // Validate email exists
            var emailExist = await _userService.GetByEmailAsync(userRequestDto.Email);
            if(emailExist != null)
            {
                throw new DuplicateException("User with this email already exists.");
            }

            AttachmentResponseDto? attachmentResponseDto = null;
            if(attachmentRequestDto != null) 
                attachmentResponseDto = await _fileService.UploadFileAsync(attachmentRequestDto);
            var entity = _mapper.Map(userRequestDto, attachmentResponseDto);
            var resultCreated  = await _userService.CreateAsync(entity);

            // Update the user's refresh token and expiry date
            var refreshTokenEntity = new RefreshToken()
            {
                UserId = resultCreated.Guid,
                Token = _tokenService.GenerateRefreshToken(),
                ExpirationDate = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays),
                IsRevoked = false,
                IsDeleted = false
            };
            await _refreshTokenService.CreateAsync(refreshTokenEntity);

            var tokenResponseDto = new TokenResponseDto()
            {
                AccessToken = _tokenService.GenerateToken(resultCreated.Guid, resultCreated.Email),
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes),
                RefreshToken = refreshTokenEntity.Token,
                RefreshTokenExpiry = refreshTokenEntity.ExpirationDate
            };

            var recordSave = await _unitOfWork.SaveChangesAsync();
            if (recordSave <= 1)
            {
                throw new DatabaseOperationException("Failed to register user & its token.");
            }

            return new LoginResponseDto()
            {
                IsStepSuccess = true,
                Token = tokenResponseDto,
                User = _mapper.MapToResponseDto(resultCreated),
                TransactionId = null
            };
        }

        public async Task<TokenResponseDto> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto)
        {
            if (refreshAccessTokenRequestDto == null) {
                throw new NullValueException("Refresh access token request cannot be null.");
            }
            var user = await _userService.GetByIdAsync(refreshAccessTokenRequestDto.UserId);
            var refreshToken 
                = (await _refreshTokenService.GetActiveRefreshTokens(refreshAccessTokenRequestDto.UserId)).FirstOrDefault(m => m.Token.Equals(refreshAccessTokenRequestDto.RefreshToken));
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided ID.");
            }

            if (refreshToken == null)
            {
                throw new RecordNotFoundException("Not found this request token.");
            }

            bool invalidToken = string.IsNullOrWhiteSpace(refreshAccessTokenRequestDto.RefreshToken) ||
                              string.IsNullOrEmpty(refreshToken.Token);
            bool expiredToken = refreshToken.IsRevoked ||
                                refreshToken.ExpirationDate < DateTime.UtcNow;
            bool diffToken = !refreshToken.Token.Equals(refreshAccessTokenRequestDto.RefreshToken, StringComparison.OrdinalIgnoreCase);
            // Validate the refresh token
            if (invalidToken || expiredToken || diffToken)
            {
                throw new InvalidCredentialException("Invalid or expired refresh token.");
            }

            TokenResponseDto response = new TokenResponseDto();
            //Revoke the old refresh token
            refreshToken.IsRevoked = true;
            // Generate new access token
            string newAccessToken = _tokenService.GenerateToken(user.Guid, user.Email);
            DateTime expiredAccessToken = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes);
            // Generate new refresh token
            string newRefreshToken = _tokenService.GenerateRefreshToken();
            DateTime expiredRefreshToken = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays);
            // Update the user's refresh token and expiry date
            var refreshTokenEntity = new RefreshToken()
            {
                UserId = user.Guid,
                Token = newRefreshToken,
                ExpirationDate = expiredRefreshToken,
                IsRevoked = false,
                IsDeleted = false
            };
            await _refreshTokenService.CreateAsync(refreshTokenEntity);
            // Save changes to the database
            var recordSave = await _unitOfWork.SaveChangesAsync();
            if (recordSave <= 0)
            {
                throw new DatabaseOperationException("Failed to update user refresh token.");
            }
            response.RefreshToken = newRefreshToken;
            response.AccessToken = newAccessToken;
            response.AccessTokenExpiry = expiredAccessToken;
            response.RefreshTokenExpiry = expiredRefreshToken;
            return response;
        }

        public async Task LogoutAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException("User ID must not be empty.");
            }
            // Revoke all refresh tokens for the user
            await _refreshTokenService.RevokeAllActiveTokens(userId);
        }

        public async Task<bool> ChangePasswordAsync(Guid currentUserId, ChangePasswordRequestDto changePasswordRequestDto)
        {
            if (changePasswordRequestDto == null)
            {
                throw new NullValueException("Change password request cannot be null.");
            }
            // Get user
            var user = await _userService.GetByIdAsync(currentUserId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User not found with the provided ID {currentUserId}");
            }
            // Validate the change password request
            if (!PasswordHasher.Verify(changePasswordRequestDto.OldPassword, user.PasswordHash))
            {
                return false;
            }
            user = _mapper.Map(changePasswordRequestDto, user);
            await _userService.UpdateAsync(user);
            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            var user = await _userService.GetByEmailAsync(forgotPasswordRequestDto.Email);
            if(user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Generate a reset token and store it in cache
            string resetToken = ItemGenerator.GenerateOtp(6);
            string key = ItemGenerator.GenerateKey(KeyCache.ResetPassword, user.Guid.ToString());
            _cacheService.Set(key, resetToken, TimeSpan.FromMinutes(15));
        }

        public async Task<LoginResponseDto> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = await _userService.GetByEmailAsync(resetPasswordRequestDto.Email);
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Validate the reset token
            string key = ItemGenerator.GenerateKey(KeyCache.ResetPassword, user.Guid.ToString());
            string resetTokenInCache = _cacheService.Get(key);
            if (string.IsNullOrWhiteSpace(resetTokenInCache) || !resetTokenInCache.Equals(resetPasswordRequestDto.ResetToken, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidCredentialException("Invalid or expired reset token.");
            }
            // Update the user's password
            user = _mapper.Map(resetPasswordRequestDto, user);
            await _userService.UpdateAsync(user);
            // Clear the reset token from cache
            _cacheService.Remove(key);
            // Generate JWT token
            string token = _tokenService.GenerateToken(user.Guid, user.Email);
            string refreshToken = _tokenService.GenerateRefreshToken();
            // Update the user's refresh token and expiry date
            RefreshToken refreshTokenNew = new RefreshToken()
            {
                UserId = user.Guid,
                Token = refreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays),
                IsRevoked = false,
                IsDeleted = false
            };
            await _refreshTokenService.CreateAsync(refreshTokenNew);
            var recordSave = await _unitOfWork.SaveChangesAsync();
            if (recordSave < 2)
            {
                throw new DatabaseOperationException("Failed to reset user password & its token.");
            }
            return new LoginResponseDto()
            {
                IsStepSuccess = true,
                Token = new TokenResponseDto()
                {
                    AccessToken = token,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes),
                    RefreshToken = refreshTokenNew.Token,
                    RefreshTokenExpiry = refreshTokenNew.ExpirationDate
                },
                User = _mapper.MapToResponseDto(user),
                TransactionId = null // Clear transaction ID after successful reset
            };
        }
    }
}
