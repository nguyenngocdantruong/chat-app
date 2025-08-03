using ChatApp.Application.DTOs.Filter;
using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.DTOs.Result;
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
using ChatApp.Shared.Utils;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ArgNullException = ChatApp.Domain.Exceptions.Runtime.ArgumentNullException;

namespace ChatApp.Application.Services
{
    public class AuthService
        (IUserService userService, IUserMapper mapper, IUnitOfWork unitOfWork,
            ICacheService<string> cacheService, IMailService mailService, ITokenService tokenService,
            IFileService fileService, ITokenSetting tokenSetting, IRefreshTokenService refreshTokenService)
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

        public async Task<Result<LoginResponseDto>> LoginFirstStep(LoginRequestDto loginRequestDto)
        {
            // Validate the login request
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || string.IsNullOrWhiteSpace(loginRequestDto.Password))
            {
                throw new ValidationException("Email and password must not be empty.");
            }
            LoginResponseDto loginResponseDto = new LoginResponseDto();
            var user = await _userService.GetByEmailAsync(loginRequestDto.Email);
            if(user == null)
            {
                return Result<LoginResponseDto>.Failure("User with that email doesn't exists");
            }

            Result<LoginResponseDto> result = new();
            //Validate the user password
            if (user.Email.Equals(loginRequestDto.Email) &&
                await _userService.ComparePasswordAsync(user.Guid, loginRequestDto.Password))
            {
                //Send email
                string otpRandom = ItemGenerator.GenerateOtp(length: 6);
                await _mailService.SendOtp(user.Email, otpRandom, ActionType.Login);
                // Generate a transaction ID for the first step of login
                string transactionId = ItemGenerator.GenerateRandom();
                await _cacheService.Set(transactionId, otpRandom, TimeSpan.FromMinutes(15));
                loginResponseDto.TransactionId = transactionId;
                result.IsSuccess = true;
                result.Data = loginResponseDto;
                result.Message = "Please input 2FA OTP code sent to your email.";
            }
            else
            {
                loginResponseDto.TransactionId = null;
                result.IsSuccess = false;
                result.Message = "Invalid email or password.";
            }
            loginResponseDto.User = null;
            loginResponseDto.Token = null;
            return result;
        }

        public async Task<Result<LoginResponseDto>> LoginWith2FaAsync(LoginRequestDto loginRequestDto)
        {
            // Validate the login request
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || string.IsNullOrWhiteSpace(loginRequestDto.Code) || string.IsNullOrEmpty(loginRequestDto.TransactionId))
            {
                throw new ValidationException("Email, code and transaction id must not be empty.");
            }

            var user = await _userService.GetByEmailAsync(loginRequestDto.Email);
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Check if the transaction ID is valid
            string otpCodeInCache = await _cacheService.Get(loginRequestDto.TransactionId);
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
            await _cacheService.Remove(loginRequestDto.TransactionId);
            // Generate JWT token
            string token = _tokenService.GenerateToken(user.Guid, user.Email);
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

            var loginResponseDto = new LoginResponseDto
            {
                Token = new TokenResponseDto()
                {
                    AccessToken = token,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays)
                },
                User = user,
                TransactionId = null // Clear transaction ID after successful login
            };
            Result<LoginResponseDto> result = new Result<LoginResponseDto>
            {
                IsSuccess = true,
                Data = loginResponseDto,
                Message = "Login successful."
            };
            return result;
        }

        public async Task ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto)
        {
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
            string oldOtp = await _cacheService.Get(resendEmailRequestDto.TransactionId);
            if (string.IsNullOrWhiteSpace(resendEmailRequestDto.TransactionId) || string.IsNullOrEmpty(oldOtp))
            {
                throw new BadRequestException("Invalid transaction ID or expired.");
            }
            // Send OTP code to the user's email
            string otpRandom = ItemGenerator.GenerateOtp(length: 6);
            await _mailService.SendOtp(user.Email, otpRandom, resendEmailRequestDto.ActionType);
            // Store the OTP code in cache with the transaction ID
            await _cacheService.Set(resendEmailRequestDto.TransactionId, otpRandom, TimeSpan.FromMinutes(15));
        }

        public async Task<Result<PreRegisterResponseDto>> PreRegisterAsync(PreRegisterRequestDto preRegisterRequestDto)
        {
            // Validate email exists
            var emailExist = await _userService.GetByEmailAsync(preRegisterRequestDto.Email);
            if (emailExist != null)
            {
                throw new DuplicateException("User with this email already exists.");
            }

            Result<PreRegisterResponseDto> result;
            //Check if the pre-register has transaction ID
            if (!string.IsNullOrEmpty(preRegisterRequestDto.TransactionId))
            {
                string otp = await _cacheService.Get(preRegisterRequestDto.TransactionId);
                // Validate the transaction ID and code
                if (string.IsNullOrWhiteSpace(preRegisterRequestDto.TransactionId) || string.IsNullOrEmpty(otp))
                {
                    throw new BadRequestException("Invalid transaction ID or expired.");
                }
                if (string.IsNullOrWhiteSpace(preRegisterRequestDto.Code) 
                    || !preRegisterRequestDto.Code.Equals(otp, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidCredentialException("Invalid verification code.");
                }
                //Successfully verified the pre-registration request
                await _cacheService.Set(preRegisterRequestDto.TransactionId, preRegisterRequestDto.Email,
                    TimeSpan.FromMinutes(5));
                result = new Result<PreRegisterResponseDto>
                {
                    IsSuccess = true,
                    Message = "Pre-registration successful. The request valid in 5 minutes.",
                    Data = new PreRegisterResponseDto
                    {
                        Email = preRegisterRequestDto.Email,
                        TransactionId = preRegisterRequestDto.TransactionId 
                    }
                };
            }
            // Create new pre-register request
            else
            {
                string transactionId = ItemGenerator.GenerateRandom();
                PreRegisterResponseDto preRegisterResponseDto = new PreRegisterResponseDto
                {
                    Email = preRegisterRequestDto.Email,
                    TransactionId = transactionId
                };
                // Generate a verification code and store it in cache
                string verificationCode = ItemGenerator.GenerateOtp(length: 6);
                await _cacheService.Set(transactionId, verificationCode, TimeSpan.FromMinutes(15));
                //Send email
                await _mailService.SendOtp(preRegisterRequestDto.Email, verificationCode, ActionType.Register);
                result = new Result<PreRegisterResponseDto>
                {
                    IsSuccess = true,
                    Message = "Please check your email for the verification code.",
                    Data = preRegisterResponseDto
                };
            }
            return result;
        }

        public async Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto userRequestDto, AttachmentRequestDto? attachmentRequestDto)
        {
            var emailRequest = await _cacheService.Get(userRequestDto.TransactionId);
            bool isValidRequest = !string.IsNullOrEmpty(emailRequest) &&
                                  userRequestDto.Email.Equals(emailRequest, StringComparison.OrdinalIgnoreCase);
            if (isValidRequest == false)
            {
                throw new BadRequestException("Register request is invalid or expired. Please start over again.");
            }

            AttachmentResponseDto? attachmentResponseDto = null;
            if (attachmentRequestDto != null)
            {
                attachmentResponseDto = await _fileService.UploadFileAsync(attachmentRequestDto);
                // Implementation of attachment upload logic

                //
            }
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
            Result<LoginResponseDto> result = new Result<LoginResponseDto>
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                Data = new LoginResponseDto
                {
                    Token = tokenResponseDto,
                    User = resultCreated,
                    TransactionId = null // Clear transaction ID after successful registration
                }
            };
            return result;
        }

        public async Task<Result<TokenResponseDto>> RefreshAccessTokenAsync(RefreshAccessTokenRequestDto refreshAccessTokenRequestDto)
        {
            TokenResponseDto newToken = await _refreshTokenService.RotationRefreshToken(refreshAccessTokenRequestDto);
            return new Result<TokenResponseDto>
            {
                IsSuccess = true,
                Message = "Access token refreshed successfully.",
                Data = newToken
            };
        }

        public async Task<Result<object>> LogoutAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException("User ID must not be empty.");
            }
            // Revoke all refresh tokens for the user
            await _refreshTokenService.RevokeAllActiveTokens(userId);
            return new Result<object>
            {
                IsSuccess = true,
                Message = "User logged out successfully.",
                Data = null
            };
        }

        public async Task<Result<object>> ChangePasswordAsync(Guid currentUserId, ChangePasswordRequestDto changePasswordRequestDto)
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
            Result<object> result = new Result<object>
            {
                IsSuccess = false,
                Message = "Change password request is invalid."
            };
            // Validate the change password request
            if (await userService.ComparePasswordAsync(user.Guid, changePasswordRequestDto.OldPassword))
            {
                return result;
            }
            await _userService.ChangePasswordAsync(user.Guid, changePasswordRequestDto);
            bool isSave = await _unitOfWork.SaveChangesAsync() > 0;
            if (!isSave)
            {
                throw new DatabaseOperationException("Failed to change user password.");
            }
            result.IsSuccess = true;
            result.Message = "User password changed successfully.";
            return result;
        }

        public async Task<Result<object>> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            var user = await _userService.GetByEmailAsync(forgotPasswordRequestDto.Email);
            if(user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Generate a reset token and store it in cache
            string resetToken = ItemGenerator.GenerateOtp(6);
            string key = ItemGenerator.GenerateKey(KeyCache.ResetPassword, user.Guid.ToString());
            await _cacheService.Set(key, resetToken, TimeSpan.FromMinutes(15));
            return new Result<object>()
            {
                Data = null,
                IsSuccess = true,
                Message = "Reset token generated successfully. Please check your email.",
            };
        }

        public async Task<Result<LoginResponseDto>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = await _userService.GetByEmailAsync(resetPasswordRequestDto.Email);
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Validate the reset token
            string key = ItemGenerator.GenerateKey(KeyCache.ResetPassword, user.Guid.ToString());
            string resetTokenInCache = await _cacheService.Get(key);
            if (string.IsNullOrWhiteSpace(resetTokenInCache) || !resetTokenInCache.Equals(resetPasswordRequestDto.ResetToken, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidCredentialException("Invalid or expired reset token.");
            }
            // Reset the user's password
            await _userService.ResetPasswordAsync(user.Guid, resetPasswordRequestDto);
            // Clear the reset token from cache
            await _cacheService.Remove(key);
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
            await _unitOfWork.SaveChangesAsync();

            Result<LoginResponseDto> result = new Result<LoginResponseDto>
            {
                IsSuccess = true,
                Message = "Password reset successfully.",
                Data = new LoginResponseDto()
                {
                    Token = new TokenResponseDto()
                    {
                        AccessToken = token,
                        AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes),
                        RefreshToken = refreshTokenNew.Token,
                        RefreshTokenExpiry = refreshTokenNew.ExpirationDate
                    },
                    User = user,
                    TransactionId = null // Clear transaction ID after successful reset
                }
            };
            return result;
        }
    }
}
