using Gym.AuthorizationServer.Entities.AccessTokens;
using Gym.AuthorizationServer.Entities.RefreshTokens;
using Gym.AuthorizationServer.Entities.UserConsents;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Shared.Abstractions;
using Idp.Services;

namespace Gym.AuthorizationServer.Services.Flows
{
    public interface IRefreshTokenFlowService
    {
        Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    }

    public class RefreshTokenFlowService(
        IRefreshTokenRepository _refreshTokenRepository,
        IAccessTokenRepository _accessTokenRepository,
        IUserConsentRepository _userConsentRepository,
        IAccessTokenGenerator _accessTokenGenerator,
        IRefreshTokenGenerator _refreshTokenGenerator) : IRefreshTokenFlowService
    {
        public async Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            RefreshTokenEntity? usedRefreshTokenEntity = await _refreshTokenRepository.ConsumeByTokenAsync(request.RefreshToken, cancellationToken);
            if (usedRefreshTokenEntity is null)
                return Result<RefreshTokenResponse>.Failure("invalid_grant", "Such refresh token not exist");

            AccessTokenEntity? usedAccessTokenEntity = await _accessTokenRepository.ConsumeByIdAsync(usedRefreshTokenEntity.AccessTokenId, cancellationToken);
            if (usedAccessTokenEntity is null)
                return Result<RefreshTokenResponse>.Failure("invalid_grant", "Access token by refresh token not exist");

            UserConsentEntity? userConsent = await _userConsentRepository
                .GetByUserIdAndClientIdAsync(usedAccessTokenEntity.UserId, usedAccessTokenEntity.ClientId, cancellationToken);
            if (userConsent is null)
                return Result<RefreshTokenResponse>.Failure("invalid_grant", "User has no consent");

            String accessToken = await _accessTokenGenerator.GenerateTokenAsync(userConsent, cancellationToken);
            AccessTokenEntity newAccessTokenEntity = new()
            {
                Token = accessToken,
                ClientId = usedAccessTokenEntity.ClientId,
                UserId = usedAccessTokenEntity.UserId,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            await _accessTokenRepository.AddAsync(newAccessTokenEntity, cancellationToken);

            String refreshToken = _refreshTokenGenerator.GenerateToken();
            RefreshTokenEntity newRefreshTokenEntity = new()
            {
                Token = refreshToken,
                AccessTokenId = newAccessTokenEntity.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

            return Result<RefreshTokenResponse>.Success(new() 
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = newAccessTokenEntity.ExpiresAt.GetSecondsFromUtcNow(),
                Scope = String.Join(' ', userConsent.GrantedScopes)
            });
        }
    }

    public record RefreshTokenRequest
    {
        public required String RefreshToken { get; init; }
    }

    public record RefreshTokenResponse
    {
        public required String AccessToken { get; init; }
        public required String TokenType { get; init; }
        public String? RefreshToken { get; init; }
        public Int32? ExpiresIn { get; init; }
        public String? Scope { get; init; }
    }
}
