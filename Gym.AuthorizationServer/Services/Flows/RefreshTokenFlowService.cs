using Gym.AuthorizationServer.Abstractions;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.AccessTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources;
using Gym.AuthorizationServer.Infrastructure.Entities.RefreshTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Services.Tokens;

namespace Gym.AuthorizationServer.Services.Flows
{
    public interface IRefreshTokenFlowService
    {
        Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
    }

    public class RefreshTokenFlowService(
        IRefreshTokenRepository _refreshTokenRepository,
        IAccessTokenRepository _accessTokenRepository,
        IProtectedResourceRepository _protectedResourceRepository,
        IUserConsentRepository _userConsentRepository,
        IAccessTokenGenerator _accessTokenGenerator,
        IRefreshTokenGenerator _refreshTokenGenerator,
        IIdTokenGeneratorHelper _idTokenGeneratorHelper,
        IUserRoleByUserIdFinder _userRoleByUserIdFinder) : IRefreshTokenFlowService
    {
        public async Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            RefreshTokenEntity? usedRefreshTokenEntity = await _refreshTokenRepository.ConsumeByTokenAsync(request.RefreshToken, cancellationToken);
            if (usedRefreshTokenEntity is null)
                return Result<RefreshTokenResponse>.Failure("invalid_grant", "Such refresh token not exist");

            AccessTokenEntity? usedAccessTokenEntity = await _accessTokenRepository.ConsumeByIdAsync(usedRefreshTokenEntity.AccessTokenId, cancellationToken);
            if (usedAccessTokenEntity is null)
                return Result<RefreshTokenResponse>.Failure("invalid_grant", "Access token by refresh token not exist");

            if(request.Resource is not null)
            {
                ProtectedResourceEntity? protectedResourceEntity = await _protectedResourceRepository.GetByAudienceUriAsync(request.Resource, cancellationToken);
                if (protectedResourceEntity is null || protectedResourceEntity.Id != usedAccessTokenEntity.ProtectedResourceId)
                    return Result<RefreshTokenResponse>.Failure("invalid_grant", "Param resource is not valid for grant_code");
            }

            UserConsentEntity? userConsent = await _userConsentRepository
                .GetAsync(usedAccessTokenEntity.UserId, usedAccessTokenEntity.ClientId, usedAccessTokenEntity.ProtectedResourceId!, cancellationToken);
            if (userConsent is null)
                return Result<RefreshTokenResponse>.Failure("invalid_grant", "User has no consent");

            var findUserRoleResult = await _userRoleByUserIdFinder.FindAsync(userConsent.UserId, cancellationToken);
            if (findUserRoleResult.IsFailed)
                return Result<RefreshTokenResponse>.Failure(findUserRoleResult.ErrorCode, findUserRoleResult.ErrorDescription);

            AccessTokenClaimsMetadata accessTokenClaimsMetadata = new()
            {
                ClientId = userConsent.ClientId,
                UserId = userConsent.UserId,
                GrantedScopes = userConsent.GrantedScopes,
                UserRole = findUserRoleResult.Value.Name
            };

            String accessToken = _accessTokenGenerator.GenerateToken(accessTokenClaimsMetadata);
            AccessTokenEntity newAccessTokenEntity = new()
            {
                Token = accessToken,
                ClientId = usedAccessTokenEntity.ClientId,
                UserId = usedAccessTokenEntity.UserId,
                ProtectedResourceId = usedAccessTokenEntity.ProtectedResourceId,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            await _accessTokenRepository.AddAsync(newAccessTokenEntity, cancellationToken);

            String refreshToken = _refreshTokenGenerator.GenerateToken();
            RefreshTokenEntity newRefreshTokenEntity = new()
            {
                Token = refreshToken,
                AccessTokenId = newAccessTokenEntity.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                Acr = usedRefreshTokenEntity.Acr,
                Amr = usedRefreshTokenEntity.Amr
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);

            String? idToken = null;
            if (userConsent.GrantedScopes.Any(aScope => aScope.Name == "openid"))
            {
                idToken = _idTokenGeneratorHelper
                    .GenerateToken(accessToken, newAccessTokenEntity.UserId, newAccessTokenEntity.ClientId, acr: newRefreshTokenEntity.Acr, amr: newRefreshTokenEntity.Amr);
            }

            return Result<RefreshTokenResponse>.Success(new() 
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = newAccessTokenEntity.ExpiresAt.GetSecondsFromUtcNow(),
                Scope = String.Join(' ', userConsent.GrantedScopes.Select(aScope => aScope.Name)),
                IdToken = idToken
            });
        }
    }

    public record RefreshTokenRequest
    {
        public required String RefreshToken { get; init; }
        public String? Resource { get; init; }
    }

    public record RefreshTokenResponse
    {
        public required String AccessToken { get; init; }
        public required String TokenType { get; init; }
        public String? RefreshToken { get; init; }
        public Int32? ExpiresIn { get; init; }
        public String? Scope { get; init; }
        public String? IdToken { get; init; }
    }
}
