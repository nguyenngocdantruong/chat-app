using ChatApp.Application.DTOs.Request;
using ChatApp.Application.DTOs.Response;
using ChatApp.Application.Interfaces.Mapper;
using ChatApp.Application.Interfaces.Services;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Exceptions.Authentication;
using ChatApp.Domain.Exceptions.Database;
using ChatApp.Domain.Interfaces;
using ChatApp.Shared.Configurations;

namespace ChatApp.Application.Services
{
    public class RefreshTokenService(
        IUnitOfWork unitOfWork,
        IUserService userService,
        ITokenService tokenService,
        ITokenSetting tokenSetting,
        IRefreshTokenMapper mapper) : GenericService<RefreshToken, RefreshTokenResponseDto>(unitOfWork, mapper), IRefreshTokenService
    {
        private readonly IUserService _userService = userService ?? throw new Domain.Exceptions.Runtime.ArgumentNullException(nameof(userService));
        private readonly ITokenService _tokenService = tokenService ?? throw new Domain.Exceptions.Runtime.ArgumentNullException(nameof(tokenService));
        private readonly ITokenSetting _tokenSetting = tokenSetting ?? throw new Domain.Exceptions.Runtime.ArgumentNullException(nameof(tokenSetting));

        public async Task<IEnumerable<RefreshTokenResponseDto>> GetActiveRefreshTokens(Guid userId)
        {
            var result = await UnitOfWork.RefreshTokenRepository.GetAllTokenActiveByUserIdAsync(userId);
            return result.Select(m => new RefreshTokenResponseDto()
            {
                Guid = m.Guid,
                Token = m.Token,
                UserId = m.UserId,
                ExpirationDate = m.ExpirationDate,
                IsRevoked = m.IsRevoked
            }).ToList();
        }

        public async Task RevokeAllActiveTokens(Guid userId)
        {
            await UnitOfWork.RefreshTokenRepository.RevokeAllTokenByUserIdAsync(userId);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task RevokeToken(Guid tokenId)
        {
            RefreshToken? token = await UnitOfWork.RefreshTokenRepository.GetByIdAsync(tokenId);
            if (token == null)
            {
                throw new RecordNotFoundException($"Refresh token not found with ID {tokenId}");
            }
            token.IsRevoked = true;
            token.UpdatedAt = DateTime.UtcNow;
        }

        public async Task<TokenResponseDto> RotationRefreshToken(RefreshAccessTokenRequestDto requestDto)
        {
            var user = await _userService.GetByIdAsync(requestDto.UserId);
            if (user == null)
            {
                throw new RecordNotFoundException($"User not found with ID {requestDto.UserId}");
            }
            var lstTokens = ((List<RefreshTokenResponseDto>)await GetActiveRefreshTokens(requestDto.UserId));
            var token = lstTokens.FirstOrDefault(m => m.Token.Equals(requestDto.RefreshToken));
            if (token != null)
            {
                if (token.Guid == null)
                {
                    throw new RecordNotFoundException($"Refresh token not found with ID {token.Guid}");
                }
                //Revoke the old token
                await RevokeToken(token.Guid.Value);
                var expRefToken = DateTime.UtcNow.AddDays(_tokenSetting.RefreshTokenExpirationDays);
                RefreshToken newRefreshToken = new RefreshToken()
                {
                    Token = _tokenService.GenerateRefreshToken(),
                    ExpirationDate = expRefToken,
                    IsRevoked = false,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UserId = requestDto.UserId
                };
                await UnitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                var recordSave = await UnitOfWork.SaveChangesAsync();
                if (recordSave < 2)
                {
                    throw new DatabaseOperationException("Cannot rotation refresh token");
                }

                TokenResponseDto tokenResponseDto = new TokenResponseDto()
                {
                    AccessToken = _tokenService.GenerateToken(requestDto.UserId, user.Email),
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_tokenSetting.ExpirationMinutes),
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiry = expRefToken,
                };
                return tokenResponseDto;
            }
            else
            {
                throw new InvalidCredentialException("Invalid token");
            }
        }
    }
}
