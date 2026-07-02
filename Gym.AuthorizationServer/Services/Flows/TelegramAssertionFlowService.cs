using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.AccessTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Clients;
using Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources;
using Gym.AuthorizationServer.Infrastructure.Entities.RefreshTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.TelegramCredentials;
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
        IProtectedResourceRepository _protectedResourceRepository,
        IScopeGrantResolveService _scopeGrantResolveService,
        IRoleRepository _roleRepository,
        IUserRepository _userRepository,
        IUpsertUserConsentService _upsertUserConsentService,
        IAccessTokenGenerator _accessTokenGenerator,
        IAccessTokenRepository _accessTokenRepository,
        IRefreshTokenGenerator _refreshTokenGenerator,
        IRefreshTokenRepository _refreshTokenRepository,
        IIdTokenGeneratorHelper _idTokenGeneratorHelper,
        IUserRoleByUserIdFinder _userRoleByUserIdFinder) : ITelegramAssertionFlowService
    {
        public async Task<Result<TelegramAssertionResponse>> HandleAsync(TelegramAssertionRequest request, CancellationToken cancellationToken)
        {
            Result<TelegramUser> verificationResult = _telegramSignatureVerifier.Verify(request.Assertion);
            if (verificationResult.IsFailed)
                return Result<TelegramAssertionResponse>.Failure("invalid_grant", "Telegram assertion hash not valid");

            ClientEntity client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken) ?? default!;
            var targetProtectedResource = await _protectedResourceRepository.GetByAudienceUriAsync(request.Resource, cancellationToken);

            TelegramCredentialEntity? telegramCredential = await _telegramCredentialRepository.GetByIdAsync(verificationResult.Value.Id, cancellationToken);
            if (telegramCredential is null)
            {
                var clientRole = await _roleRepository.GetByNameAsync("Client", cancellationToken);
                UserEntity newUser = new()
                {
                    FirstName = verificationResult.Value.FirstName,
                    LastName = verificationResult.Value.LastName,
                    RoleId = clientRole!.Id
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

            var user = await _userRepository.GetByIdAsync(telegramCredential.UserId, cancellationToken);
            var scopeResolveResult = await _scopeGrantResolveService.Resolve(user!.RoleId, targetProtectedResource!.Id, request.Scope, cancellationToken);

            UserConsentEntity userConsent = await _upsertUserConsentService.UpsertAsync(
                scopeResolveResult.Value,
                telegramCredential.UserId,
                request.ClientId,
                targetProtectedResource.Id,
                cancellationToken);

            var findUserRoleResult = await _userRoleByUserIdFinder.FindAsync(userConsent.UserId, cancellationToken);
            if (findUserRoleResult.IsFailed)
                return Result<TelegramAssertionResponse>.Failure(findUserRoleResult.ErrorCode, findUserRoleResult.ErrorDescription);

            AccessTokenClaimsMetadata accessTokenClaimsMetadata = new()
            {
                ClientId = userConsent.ClientId,
                UserId = userConsent.UserId,
                GrantedScopes = userConsent.GrantedScopes,
                UserRole = findUserRoleResult.Value.Name
            };

            String accessToken = _accessTokenGenerator.GenerateToken(accessTokenClaimsMetadata);
            AccessTokenEntity accessTokenEntity = new()
            {
                Token = accessToken,
                ClientId = userConsent.ClientId,
                UserId = userConsent.UserId,
                ProtectedResourceId = targetProtectedResource.Id,
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
            if (userConsent.GrantedScopes.Any(aScope => aScope.Name == "openid"))
            {
                idToken = _idTokenGeneratorHelper.GenerateToken(accessToken, accessTokenEntity.UserId, accessTokenEntity.ClientId, acr: "2fa", amr: ["tel"]);
            }

            return Result<TelegramAssertionResponse>.Success(new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer",
                ExpiresIn = accessTokenEntity.ExpiresAt.GetSecondsFromUtcNow(),
                Scope = String.Join(' ', userConsent.GrantedScopes.Select(aScope => aScope.Name)),
                IdToken = idToken
            });
        }
    }

    public record TelegramAssertionRequest
    {
        public required String ClientId { get; init; }
        public required String Scope { get; init; }
        public required String Assertion { get; init; }
        public required String Resource { get; init; }
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
