using Gym.AuthorizationServer.Entities.AccessTokens;
using Gym.AuthorizationServer.Entities.Clients;
using Gym.AuthorizationServer.Entities.RefreshTokens;
using Gym.AuthorizationServer.Entities.UserConsents;
using Gym.AuthorizationServer.Entities.Users;
using Gym.AuthorizationServer.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Services;
using Idp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Integration.Tests
{
    public partial class TestServerFixture : WebApplicationFactory<Program>
    {
        public const String DefaultTestDatabase = "test-auth-server";

        public const String DefaultClientId = "65a3b5c7d8e9f0123456789a";
        public const String DefaultClientSecret = "client_secret";
        public const String DefaultClientSecretHash = "$argon2id$v=19$m=65536,t=3,p=1$sVb/rkZ+lXmGUw7XBJOEmw$K5K7R8zOJLiVM5I6NKydkC4RNYLOjCR1PcLMimqZrWE";
        public const String DefaultClientRedirectUri = "_testing/callback";

        public const String DefaultUserId = "6a1018950edd40bab32c5ff6";
        public const String DefaultUserPassword = "0cdSxf8N";
        public const String DefaultUserPasswordHash = "$argon2id$v=19$m=65536,t=3,p=1$noNLbw4SkUaAHOw5k3FLsw$AZtUtMu/yVKlsw9S3tq+ORvMivJzS7vCI7ta9lcwQhU";
        public const String DefaultUserUsername = "john_doe";

        public const String DefaultState = "test_state";

        public const String DefaultTelegramAssertion = "tgWebAppData=user%3D%257B%2522id%2522%253A5265776755%252C%2522first_name%2522%253A%2522Andrey%2522%252C%2522last_name%2522%253A%2522Kardashian%2522%252C%2522username%2522%253A%2522john_rogers_zhuzhma%2522%252C%2522language_code%2522%253A%2522en%2522%252C%2522allows_write_to_pm%2522%253Atrue%252C%2522photo_url%2522%253A%2522https%253A%255C%252F%255C%252Ft.me%255C%252Fi%255C%252Fuserpic%255C%252F320%255C%252FBoKwKY5IpDRWz6uIog74D57Ss61WeT4Jf5zTwZZS-1eHFwRJR56ePM-6SrkBKkaV.svg%2522%257D%26chat_instance%3D4756907096746764373%26chat_type%3Dprivate%26auth_date%3D1766844573%26signature%3DeNGfNiBcoU6lw7RV9SNMKxZNVfpltKsRWBG0MwDi1mzuLHnq0z_KkCRFnThl071xnR7cyWZ_IxcS2pz80NFHBQ%26hash%3D95553035c77fe4aa746125e758e8393d89e8fd15b18ec8de1471903d4ce1d044";

        public async Task ClearDatabaseAsync()
        {
            var database = Services.GetRequiredService<IMongoDatabase>();

            var collectionNames = await database.ListCollectionNamesAsync();
            foreach (var aCollectionName in await collectionNames.ToListAsync())
            {
                await database.DropCollectionAsync(aCollectionName, TestContext.Current.CancellationToken);
            }
        }

        public async Task<ClientEntity> CreateClientAsync()
        {
            using var scope = Services.CreateScope();
            var clientRepository = scope.ServiceProvider.GetRequiredService<IClientRepository>();

            ClientEntity client = new()
            {
                Id = DefaultClientId,
                Name = "test_client",
                RedirectUri = DefaultClientRedirectUri,
                Scope = ["scope_1", "scope_2"],
                SecretHash = DefaultClientSecretHash
            };

            await clientRepository.AddAsync(client, TestContext.Current.CancellationToken);
            return client;
        }

        public async Task<UserEntity> CreateUserAsync()
        {
            using var scope = Services.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            UserEntity user = new()
            {
                Id = DefaultUserId,
                FirstName = "John",
                LastName = "Doe",
                Role = "client"
            };

            await userRepository.AddAsync(user, TestContext.Current.CancellationToken);
            return user;
        }

        public async Task<FormCredentialEntity> CreateUserFormCredentialsAsync()
        {
            using var scope = Services.CreateScope();
            var formCredentialRepository = scope.ServiceProvider.GetRequiredService<IFormCredentialRepository>();

            FormCredentialEntity credentials = new()
            {
                UserId = DefaultUserId,
                Username = DefaultUserUsername,
                HashedPassword = DefaultUserPasswordHash
            };

            await formCredentialRepository.AddAsync(credentials, TestContext.Current.CancellationToken);
            return credentials;
        }

        public async Task<UserConsentEntity> CreateUserConsentAsync()
        {
            using var scope = Services.CreateScope();
            var userConsentsRepository = scope.ServiceProvider.GetRequiredService<IUserConsentRepository>();

            UserConsentEntity consent = new()
            {
                ClientId = DefaultClientId,
                UserId = DefaultUserId,
                GrantedScopes = ["scope_1", "scope_2"]
            };

            await userConsentsRepository.AddAsync(consent, TestContext.Current.CancellationToken);
            return consent;
        }

        public async Task<(String accessToken, String refreshToken)> CreateTokenPairAsync()
        {
            using var scope = Services.CreateScope();

            var userConsent = await scope.ServiceProvider
                .GetRequiredService<IUserConsentRepository>()
                .GetByUserIdAndClientIdAsync(DefaultUserId, DefaultClientId, TestContext.Current.CancellationToken);

            var accessTokenCode = await scope.ServiceProvider
                .GetRequiredService<IAccessTokenGenerator>()
                .GenerateTokenAsync(userConsent!, TestContext.Current.CancellationToken);

            AccessTokenEntity accessTokenEntity = new()
            {
                Token = accessTokenCode,
                ClientId = userConsent!.ClientId,
                UserId = userConsent.UserId!,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            await scope.ServiceProvider
                .GetRequiredService<IAccessTokenRepository>()
                .AddAsync(accessTokenEntity, TestContext.Current.CancellationToken);

            var refreshTokenCode = scope.ServiceProvider
              .GetRequiredService<IRefreshTokenGenerator>()
              .GenerateToken();

            RefreshTokenEntity refreshTokenEntity = new()
            {
                Token = refreshTokenCode,
                AccessTokenId = accessTokenEntity.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
            };
            await scope.ServiceProvider
                .GetRequiredService<IRefreshTokenRepository>()
                .AddAsync(refreshTokenEntity, TestContext.Current.CancellationToken);

            return (accessToken: accessTokenCode, refreshToken: refreshTokenCode);
        }

    }
}
