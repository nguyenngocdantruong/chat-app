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
using ChatApp.Shared.Common;
using ChatApp.Shared.Configurations;
using ChatApp.Shared.Constants;
using ChatApp.Shared.Security;
using ChatApp.Shared.Utils;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using ArgNullException = ChatApp.Domain.Exceptions.Runtime.ArgumentNullException;

namespace ChatApp.Application.Services
{
    public class AuthService(
        IUserRepository userRepository,
        IUserService userService,
        IUserMapper mapper,
        IUnitOfWork unitOfWork,
        ICacheService<string> cacheService,
        IMailService mailService,
        ITokenService tokenService,
        IFileService fileService,
        ITokenSetting tokenSetting,
        IRefreshTokenService refreshTokenService,
        ICurrentUserService currentUserService) : IAuthService
    {

        public async Task<Result<LoginResponseDto>> LoginFirstStep(LoginRequestDto loginRequestDto)
        {
            // Validate the login request
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email) || string.IsNullOrWhiteSpace(loginRequestDto.Password))
            {
                throw new ValidationException("Email and password must not be empty.");
            }
            LoginResponseDto loginResponseDto = new LoginResponseDto();
            var user = (await userService.GetByEmailAsync(loginRequestDto.Email)).Data;
            if (user == null)
            {
                return Result<LoginResponseDto>.Failure("User with that email doesn't exists");
            }

            Result<LoginResponseDto> result = new();
            //Validate the user password
            if (user.Email != null && user.Email.Equals(loginRequestDto.Email) &&
                await userService.ComparePasswordAsync(user.Guid, loginRequestDto.Password))
            {
                //Send email
                string otpRandom = ItemGenerator.GenerateOtp(length: 6);
                await mailService.SendOtp(user.Email, otpRandom, ActionType.Login);
                // Generate a transaction ID for the first step of login
                string transactionId = ItemGenerator.GenerateRandom();
                await cacheService.Set(transactionId, otpRandom, TimeSpan.FromMinutes(15));
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

            var user = (await userService.GetByEmailAsync(loginRequestDto.Email)).Data;
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Check if the transaction ID is valid
            string otpCodeInCache = await cacheService.Get(loginRequestDto.TransactionId);
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
            await cacheService.Remove(loginRequestDto.TransactionId);
            // Generate JWT token
            string token = tokenService.GenerateToken(user.Guid, user.Email);
            string refreshToken = tokenService.GenerateRefreshToken();

            // Update the user's refresh token and expiry date
            await refreshTokenService.CreateAsync(new RefreshToken()
            {
                UserId = user.Guid,
                Token = refreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(tokenSetting.RefreshTokenExpirationDays),
                IsRevoked = false,
                IsDeleted = false
            });

            await unitOfWork.SaveChangesAsync();

            var loginResponseDto = new LoginResponseDto
            {
                Token = new TokenResponseDto()
                {
                    AccessToken = token,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(tokenSetting.ExpirationMinutes),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(tokenSetting.RefreshTokenExpirationDays)
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

        public async Task<Result<object>> ResendEmailAsync(ResendEmailRequestDto resendEmailRequestDto)
        {
            // Validate the resend email request
            if (string.IsNullOrWhiteSpace(resendEmailRequestDto.Email) || string.IsNullOrWhiteSpace(resendEmailRequestDto.TransactionId))
            {
                throw new ValidationException("Email and transaction ID must not be empty.");
            }
            var user = (await userService.GetByEmailAsync(resendEmailRequestDto.Email)).Data;
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            //Check if the transaction ID is valid
            string oldOtp = await cacheService.Get(resendEmailRequestDto.TransactionId);
            if (string.IsNullOrWhiteSpace(resendEmailRequestDto.TransactionId) || string.IsNullOrEmpty(oldOtp))
            {
                throw new BadRequestException("Invalid transaction ID or expired.");
            }
            // Send OTP code to the user's email
            string otpRandom = ItemGenerator.GenerateOtp(length: 6);
            await mailService.SendOtp(user.Email, otpRandom, resendEmailRequestDto.ActionType);
            // Store the OTP code in cache with the transaction ID
            await cacheService.Set(resendEmailRequestDto.TransactionId, otpRandom, TimeSpan.FromMinutes(15));
            return new Result<object>
            {
                IsSuccess = true,
                Message = "Verification code sent successfully.",
                Data = null // No data to return
            };
        }

        public async Task<Result<PreRegisterResponseDto>> PreRegisterAsync(PreRegisterRequestDto preRegisterRequestDto)
        {
            // Validate email exists
            var emailExist = await userService.GetByEmailAsync(preRegisterRequestDto.Email);
            if (emailExist != null)
            {
                throw new DuplicateException("User with this email already exists.");
            }

            Result<PreRegisterResponseDto> result;
            //Check if the pre-register has transaction ID
            if (!string.IsNullOrEmpty(preRegisterRequestDto.TransactionId))
            {
                string otp = await cacheService.Get(preRegisterRequestDto.TransactionId);
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
                await cacheService.Set(preRegisterRequestDto.TransactionId, preRegisterRequestDto.Email,
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
                await cacheService.Set(transactionId, verificationCode, TimeSpan.FromMinutes(15));
                //Send email
                await mailService.SendOtp(preRegisterRequestDto.Email, verificationCode, ActionType.Register);
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
            var emailRequest = await cacheService.Get(userRequestDto.TransactionId);
            bool isValidRequest = !string.IsNullOrEmpty(emailRequest) &&
                                  userRequestDto.Email.Equals(emailRequest, StringComparison.OrdinalIgnoreCase);
            if (isValidRequest == false)
            {
                throw new BadRequestException("Register request is invalid or expired. Please start over again.");
            }

            AttachmentResponseDto? attachmentResponseDto = null;
            if (attachmentRequestDto != null)
            {
                attachmentResponseDto = await fileService.UploadFileAsync(attachmentRequestDto);
            }
            var entity = mapper.Map(userRequestDto, attachmentResponseDto);
            var resultCreated = await userService.CreateAsync(entity);

            // Update the user's refresh token and expiry date
            var refreshTokenEntity = new RefreshToken()
            {
                UserId = resultCreated.Guid,
                Token = tokenService.GenerateRefreshToken(),
                ExpirationDate = DateTime.UtcNow.AddDays(tokenSetting.RefreshTokenExpirationDays),
                IsRevoked = false,
                IsDeleted = false
            };
            await refreshTokenService.CreateAsync(refreshTokenEntity);

            var tokenResponseDto = new TokenResponseDto()
            {
                AccessToken = tokenService.GenerateToken(resultCreated.Guid, resultCreated.Email),
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(tokenSetting.ExpirationMinutes),
                RefreshToken = refreshTokenEntity.Token,
                RefreshTokenExpiry = refreshTokenEntity.ExpirationDate
            };

            await unitOfWork.SaveChangesAsync();

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
            TokenResponseDto newToken = await refreshTokenService.RotationRefreshToken(refreshAccessTokenRequestDto);
            await unitOfWork.SaveChangesAsync();
            return new Result<TokenResponseDto>
            {
                IsSuccess = true,
                Message = "Access token refreshed successfully.",
                Data = newToken
            };
        }

        public async Task<Result<object>> LogoutAsync()
        {
            var userId = currentUserService.UserId;
            // Revoke all refresh tokens for the user
            await refreshTokenService.RevokeAllActiveTokens(userId.Value);
            await unitOfWork.SaveChangesAsync();
            return new Result<object>
            {
                IsSuccess = true,
                Message = "User logged out successfully.",
                Data = null
            };
        }

        public async Task<Result<object>> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequestDto)
        {
            // Validate the old password
            if (!await userService.ComparePasswordAsync(changePasswordRequestDto.UserId, changePasswordRequestDto.OldPassword))
            {
                return new Result<object>
                {
                    IsSuccess = false,
                    Message = "Invalid old password."
                };
            }

            // Change the user's password
            var user = await userRepository.GetByUID(changePasswordRequestDto.UserId);
            if (user == null)
            {
                throw new RecordNotFoundException("User not found.");
            }
            user = mapper.Map(changePasswordRequestDto, user);
            await userService.UpdateAsync(user.Guid, user);

            await unitOfWork.SaveChangesAsync();

            return new Result<object>
            {
                IsSuccess = true,
                Message = "User password changed successfully.",
                Data = null
            };
        }

        public async Task<Result<object>> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            var user = (await userService.GetByEmailAsync(forgotPasswordRequestDto.Email)).Data;
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Generate a reset token and store it in cache
            string resetToken = ItemGenerator.GenerateOtp(6);
            string key = ItemGenerator.GenerateKey(KeyCache.ResetPassword, user.Guid.ToString());
            await cacheService.Set(key, resetToken, TimeSpan.FromMinutes(15));

            // Send reset token via email
            await mailService.SendOtp(user.Email, resetToken, ActionType.ResetPassword);

            return new Result<object>()
            {
                Data = null,
                IsSuccess = true,
                Message = "Reset token generated successfully. Please check your email.",
            };
        }

        public async Task<Result<LoginResponseDto>> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var user = (await userService.GetByEmailAsync(resetPasswordRequestDto.Email)).Data;
            if (user == null)
            {
                throw new RecordNotFoundException("User not found with the provided email.");
            }
            // Validate the reset token
            string key = ItemGenerator.GenerateKey(KeyCache.ResetPassword, user.Guid.ToString());
            string resetTokenInCache = await cacheService.Get(key);
            if (string.IsNullOrEmpty(resetTokenInCache) || !resetTokenInCache.Equals(resetPasswordRequestDto.ResetToken, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidCredentialException("Invalid or expired reset token.");
            }
            // Reset the user's password
            var userEntity = await userRepository.GetByUID(user.Guid);
            if (userEntity == null)
            {
                throw new RecordNotFoundException("User not found after password reset.");
            }
            userEntity = mapper.Map(resetPasswordRequestDto, userEntity);
            await userService.UpdateAsync(userEntity.Guid, userEntity);
            // Clear the reset token from cache
            await cacheService.Remove(key);
            // Generate JWT token
            string token = tokenService.GenerateToken(user.Guid, user.Email);
            string refreshToken = tokenService.GenerateRefreshToken();
            // Update the user's refresh token and expiry date
            RefreshToken refreshTokenNew = new RefreshToken()
            {
                UserId = user.Guid,
                Token = refreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(tokenSetting.RefreshTokenExpirationDays),
                IsRevoked = false,
                IsDeleted = false
            };
            await refreshTokenService.CreateAsync(refreshTokenNew);

            await unitOfWork.SaveChangesAsync();

            Result<LoginResponseDto> result = new Result<LoginResponseDto>
            {
                IsSuccess = true,
                Message = "Password reset successfully.",
                Data = new LoginResponseDto()
                {
                    Token = new TokenResponseDto()
                    {
                        AccessToken = token,
                        AccessTokenExpiry = DateTime.UtcNow.AddMinutes(tokenSetting.ExpirationMinutes),
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
