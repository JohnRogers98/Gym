using Gym.AuthorizationServer.Entities.AccessTokens;
using Gym.AuthorizationServer.Entities.GrantCodes;
using Gym.AuthorizationServer.Entities.RefreshTokens;
using Gym.AuthorizationServer.Entities.UserConsents;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Services.Tokens;
using Gym.AuthorizationServer.Shared.Abstractions;

namespace Gym.AuthorizationServer.Services.Flows
{
    public interface IAuthorizationCodeFlowService
    {
        Task<Result<AuthorizationCodeResponse>> HandleAsync(AuthorizationCodeRequest request, CancellationToken cancellationToken); 
    }

    public class AuthorizationCodeFlowService(
        IGrantCodeRepository _grantCodeRepository,
        ICodeChallangeVerifier _codeChallangeVerifier,
        IUserConsentRepository _userConsentRepository,
        IAccessTokenGenerator _accessTokenGenerator,
        IAccessTokenRepository _accessTokenRepository,
        IRefreshTokenGenerator _refreshTokenGenerator,
        IRefreshTokenRepository _refreshTokenRepository,
        IIdTokenGeneratorHelper _idTokenGeneratorHelper) : IAuthorizationCodeFlowService
    {
        public async Task<Result<AuthorizationCodeResponse>> HandleAsync(AuthorizationCodeRequest request, CancellationToken cancellationToken)
        {
            GrantCodeEntity? grantCode = await _grantCodeRepository.ConsumeByCodeAsync(request.Code, cancellationToken);
            if (grantCode is null || grantCode.ClientId != request.ClientId)
                return Result<AuthorizationCodeResponse>.Failure("invalid_grant", "Code has not been granted");

            if (grantCode.CodeChallenge is not null)
            {
                if (request.CodeVerifier is null)
                    return Result<AuthorizationCodeResponse>.Failure("invalid_grant", "Code requires verifier");

                var pkceVerificationResult = _codeChallangeVerifier.Verify(request.CodeVerifier, grantCode.CodeChallenge, grantCode.CodeChallengeMethod!);
                if (pkceVerificationResult is false)
                    return Result<AuthorizationCodeResponse>.Failure("invalid_grant", "Code verification failed");
            }

            UserConsentEntity? userConsent = await _userConsentRepository.GetByUserIdAndClientIdAsync(
                grantCode.UserId, grantCode.ClientId, cancellationToken);
            if (userConsent is null)
                return Result<AuthorizationCodeResponse>.Failure("invalid_grant", "User has no consent");

            String accessToken = _accessTokenGenerator.GenerateToken(userConsent);
            AccessTokenEntity accessTokenEntity = new()
            {
                Token = accessToken,
                ClientId = grantCode.ClientId,
                UserId = grantCode.UserId,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            await _accessTokenRepository.AddAsync(accessTokenEntity, cancellationToken);

            String refreshToken = _refreshTokenGenerator.GenerateToken();
            RefreshTokenEntity refreshTokenEntity = new()
            {
                Token = refreshToken,
                AccessTokenId = accessTokenEntity.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                Acr = "1fa",
                Amr = ["pwd"]
            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            String? idToken = null;
            if (userConsent.GrantedScopes.Contains("openid"))
            {
                idToken = _idTokenGeneratorHelper.GenerateToken(accessToken, grantCode.UserId, grantCode.ClientId, grantCode.Nonce, "1fa", ["pwd"]);
            }

            return Result<AuthorizationCodeResponse>.Success( new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = accessTokenEntity.ExpiresAt.GetSecondsFromUtcNow(),
                Scope = String.Join(' ', userConsent.GrantedScopes),
                IdToken = idToken
            });
        }

    }

    public record AuthorizationCodeRequest
    {
        public required String ClientId { get; init; }
        public required String Code { get; init; }
        public String? CodeVerifier { get; init; }
    }

    public record AuthorizationCodeResponse
    {
        public required String AccessToken { get; init; }
        public required String TokenType { get; init; }
        public String? RefreshToken { get; init; }
        public Int32? ExpiresIn { get; init; }
        public String? Scope { get; init; }
        public String? IdToken { get; init; }
    }
}
