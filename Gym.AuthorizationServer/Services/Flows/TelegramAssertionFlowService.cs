using Gym.AuthorizationServer.Entities.AccessTokens;
using Gym.AuthorizationServer.Entities.Clients;
using Gym.AuthorizationServer.Entities.RefreshTokens;
using Gym.AuthorizationServer.Entities.UserConsents;
using Gym.AuthorizationServer.Entities.Users;
using Gym.AuthorizationServer.Entities.Users.TelegramCredentials;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Services.Tokens;
using Gym.AuthorizationServer.Shared.Abstractions;

namespace Gym.AuthorizationServer.Services.Flows
{
    public interface ITelegramAssertionFlowService
    {
        Task<Result<TelegramAssertionResponse>> HandleAsync(TelegramAssertionRequest request, CancellationToken cancellationToken);
    }

    public class TelegramAssertionFlowService(
        ITelegramSignatureVerifier _telegramSignatureVerifier,
        ITelegramCredentialRepository _telegramCredentialRepository,
        IClientRepository _clientRepository,
        IScopeChecker _scopeChecker,
        IUserRepository _userRepository,
        IUpsertUserConsentService _upsertUserConsentService,
        IAccessTokenGenerator _accessTokenGenerator,
        IAccessTokenRepository _accessTokenRepository,
        IRefreshTokenGenerator _refreshTokenGenerator,
        IRefreshTokenRepository _refreshTokenRepository,
        IIdTokenGeneratorHelper _idTokenGeneratorHelper) : ITelegramAssertionFlowService
    {
        public async Task<Result<TelegramAssertionResponse>> HandleAsync(TelegramAssertionRequest request, CancellationToken cancellationToken)
        {
            Result<TelegramUser> verificationResult = _telegramSignatureVerifier.Verify(request.Assertion);
            if (verificationResult.IsFailed)
                return Result<TelegramAssertionResponse>.Failure("invalid_grant", "Telegram assertion hash not valid");

            ClientEntity clientEntity = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken) ?? default!;
            var checkResult = _scopeChecker.CheckScopes(clientEntity.Scope is null ? null : String.Join(' ', clientEntity.Scope), request.Scope);
            if (checkResult is false)
                return Result<TelegramAssertionResponse>.Failure("invalid_scope", "Such scopes not defined for client");

            TelegramCredentialEntity? telegramCredential = await _telegramCredentialRepository.GetByIdAsync(verificationResult.Value.Id, cancellationToken);
            if (telegramCredential is null)
            {
                UserEntity newUser = new()
                {
                    FirstName = verificationResult.Value.FirstName,
                    LastName = verificationResult.Value.LastName,
                    Role = "Client"
                };
                await _userRepository.AddAsync(newUser, cancellationToken);

                telegramCredential = new()
                {
                    Id = verificationResult.Value.Id,
                    TelegramUsername = verificationResult.Value.Username,
                    UserId = newUser.Id
                };
                await _telegramCredentialRepository.AddAsync(telegramCredential, cancellationToken);
            }

            UserConsentEntity userConsent = await _upsertUserConsentService
                .UpsertAsync(request.Scope.Split(' ').ToList(), request.ClientId, telegramCredential.UserId, cancellationToken);

            String accessToken = _accessTokenGenerator.GenerateToken(userConsent);
            AccessTokenEntity accessTokenEntity = new()
            {
                Token = accessToken,
                ClientId = userConsent.ClientId,
                UserId = userConsent.UserId,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            await _accessTokenRepository.AddAsync(accessTokenEntity, cancellationToken);

            String refreshToken = _refreshTokenGenerator.GenerateToken();
            RefreshTokenEntity refreshTokenEntity = new()
            {
                Token = refreshToken,
                AccessTokenId = accessTokenEntity.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                Acr = "2fa",
                Amr = ["tel"]
            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            String? idToken = null;
            if (userConsent.GrantedScopes.Contains("openid"))
            {
                idToken = _idTokenGeneratorHelper.GenerateToken(accessToken, accessTokenEntity.UserId, accessTokenEntity.ClientId, acr: "2fa", amr: ["tel"]);
            }

            return Result<TelegramAssertionResponse>.Success(new()
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

    public record TelegramAssertionRequest
    {
        public required String ClientId { get; init; }
        public required String Scope { get; init; }
        public required String Assertion { get; init; }
    }

    public record TelegramAssertionResponse
    {
        public required String AccessToken { get; init; }
        public required String TokenType { get; init; }
        public String? RefreshToken { get; init; }
        public Int32? ExpiresIn { get; init; }
        public String? Scope { get; init; }
        public String? IdToken { get; init; }
    }
}
