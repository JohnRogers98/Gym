using Gym.AuthorizationServer.Infrastructure.Entities.AccessTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Clients;
using Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources;
using Gym.AuthorizationServer.Infrastructure.Entities.RefreshTokens;
using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using Gym.AuthorizationServer.Infrastructure.Entities.Scopes;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Services.Tokens;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Integration.Tests
{
    public partial class TestServerFixture : WebApplicationFactory<Program>
    {
        public const String DefaultTestDatabase = "test-auth-server";
        public const String DefaultHost = "https://localhost";
        public const String DefaultTokenIssuer = "https://localhost:7218";

        public async Task ClearDatabaseAsync()
        {
            var database = Services.GetRequiredService<IMongoDatabase>();

            var collectionNames = await database.ListCollectionNamesAsync();
            foreach (var aCollectionName in await collectionNames.ToListAsync())
            {
                await database.DropCollectionAsync(aCollectionName, TestContext.Current.CancellationToken);
            }
        }
    }

    public partial class DatabaseShaper : IDisposable
    {
        private TestServerFixture _fixture;
        private IServiceScope _serviceScope;

        public DatabaseShaper(TestServerFixture fixture) 
        {
            _fixture = fixture;
            _serviceScope = _fixture.Services.CreateScope();
        }

        #region With
        public async Task<ClientEntity> WithClientAsync(ClientEntity client)
        {
            var clientRepository = _serviceScope.ServiceProvider.GetRequiredService<IClientRepository>();
            await clientRepository.AddAsync(client, TestContext.Current.CancellationToken);
            return client;
        }

        public async Task<UserRoleEntity> WithUserRoleAsync(UserRoleEntity userRole)
        {
            var roleRepository = _serviceScope.ServiceProvider.GetRequiredService<IRoleRepository>();
            await roleRepository.AddAsync(userRole, TestContext.Current.CancellationToken);
            return userRole;
        }

        public async Task<UserEntity> WithUserAsync(UserEntity user)
        {
            var userRepository = _serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
            await userRepository.AddAsync(user, TestContext.Current.CancellationToken);
            return user;
        }

        public async Task<FormCredentialEntity> WithUserFormCredentialsAsync(FormCredentialEntity formCredential)
        {
            var formCredentialRepository = _serviceScope.ServiceProvider.GetRequiredService<IFormCredentialRepository>();
            await formCredentialRepository.AddAsync(formCredential, TestContext.Current.CancellationToken);
            return formCredential;
        }

        public async Task<ProtectedResourceEntity> WithProtectedResourceAsync(ProtectedResourceEntity protectedResource)
        {
            var protectedResourceRepository = _serviceScope.ServiceProvider.GetRequiredService<IProtectedResourceRepository>();
            await protectedResourceRepository.AddAsync(protectedResource, TestContext.Current.CancellationToken);
            return protectedResource;
        }

        public async Task<ScopeEntity> WithScopeAsync(ScopeEntity scope)
        {
            var scopeRepository = _serviceScope.ServiceProvider.GetRequiredService<IScopeRepository>();
            await scopeRepository.AddAsync(scope, TestContext.Current.CancellationToken);
            return scope;
        }

        public async Task<UserConsentEntity> WithUserConsentAsync(UserConsentEntity consent)
        {
            var userConsentsRepository = _serviceScope.ServiceProvider.GetRequiredService<IUserConsentRepository>();
            await userConsentsRepository.AddAsync(consent, TestContext.Current.CancellationToken);
            return consent;
        }

        public async Task<AccessTokenEntity> WithAccessTokenAsync(AccessTokenEntity accessToken)
        {
            var accessTokenRepository = _serviceScope.ServiceProvider.GetRequiredService<IAccessTokenRepository>();
            await accessTokenRepository.AddAsync(accessToken, TestContext.Current.CancellationToken);
            return accessToken;
        }

        public async Task<RefreshTokenEntity> WithRefreshTokenAsync(RefreshTokenEntity refreshToken)
        {
            var refreshTokenRepository = _serviceScope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            await refreshTokenRepository.AddAsync(refreshToken, TestContext.Current.CancellationToken);
            return refreshToken;
        }
        #endregion

        #region With default
        public async Task<ClientEntity> WithDefaultClientAsync()
        {
            ClientEntity client = new()
            {
                Id = DefaultClientId,
                Name = "test_client",
                RedirectUri = DefaultClientRedirectUri,
                SecretHash = DefaultClientSecretHash
            };

            return await this.WithClientAsync(client);
        }

        public async Task<UserEntity> WithDefaultUserAsync()
        {
            UserEntity user = new()
            {
                Id = DefaultUserId,
                FirstName = "John",
                LastName = "Doe",
                RoleId = DefaultRoleId
            };
            return await this.WithUserAsync(user);
        }

        public async Task<UserRoleEntity> WithDefaultUserRoleAsync()
        {
            UserRoleEntity role = new()
            {
                Id = DefaultRoleId,
                Name = "Client",
            };
            return await this.WithUserRoleAsync(role);
        }

        public async Task<ProtectedResourceEntity> WithDefaultProtectedResourceAsync()
        {
            ProtectedResourceEntity protectedResource = new()
            {
                Id = DefaultProtectedResourceId,
                Name = "Test resource",
                AudienceUri = DefaultProtectedResourceAudienceUri
            };
            return await this.WithProtectedResourceAsync(protectedResource);
        }

        public async Task<ScopeEntity> WithDefaultScopeAsync()
        {
            ScopeEntity scope = new()
            {
                Id = DefaultScopeId,
                Name = "profile",
                RoleId = DefaultRoleId,
                ProtectedResourceId = DefaultProtectedResourceId,
            };
            return await this.WithScopeAsync(scope);
        }

        public async Task<FormCredentialEntity> WithDefaultUserFormCredentialsAsync()
        {
            FormCredentialEntity credentials = new()
            {
                UserId = DefaultUserId,
                Username = DefaultUserUsername,
                HashedPassword = DefaultUserPasswordHash
            };

            await this.WithUserFormCredentialsAsync(credentials);
            return credentials;
        }

        public async Task<UserConsentEntity> WithDefaultUserConsentAsync()
        {
            UserConsentEntity consent = new()
            {
                ClientId = DefaultClientId,
                UserId = DefaultUserId,
                ProtectedResourceId = DefaultProtectedResourceId,
                GrantedScopes = [new() { Id = DefaultScopeId, Name = "scope_1" }]
            };
            return await this.WithUserConsentAsync(consent);
        }

        public async Task<(String accessToken, String refreshToken)> WithDefaultTokenPairAsync()
        {
            var userConsent = await _serviceScope.ServiceProvider
                .GetRequiredService<IUserConsentRepository>()
                .GetAsync(DefaultUserId, DefaultClientId, DefaultProtectedResourceId, TestContext.Current.CancellationToken);

            var accessTokenCode = _serviceScope.ServiceProvider
                .GetRequiredService<IAccessTokenGenerator>()
                .GenerateToken(userConsent!.ToAccessTokenClaimsMetadata());

            AccessTokenEntity accessTokenEntity = new()
            {
                Token = accessTokenCode,
                ClientId = userConsent!.ClientId,
                UserId = userConsent.UserId!,
                ProtectedResourceId = userConsent!.ProtectedResourceId,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            await _serviceScope.ServiceProvider
                .GetRequiredService<IAccessTokenRepository>()
                .AddAsync(accessTokenEntity, TestContext.Current.CancellationToken);

            var refreshTokenCode = _serviceScope.ServiceProvider
              .GetRequiredService<IRefreshTokenGenerator>()
              .GenerateToken();

            RefreshTokenEntity refreshTokenEntity = new()
            {
                Token = refreshTokenCode,
                AccessTokenId = accessTokenEntity.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
            };
            await _serviceScope.ServiceProvider
                .GetRequiredService<IRefreshTokenRepository>()
                .AddAsync(refreshTokenEntity, TestContext.Current.CancellationToken);

            return (accessToken: accessTokenCode, refreshToken: refreshTokenCode);
        }
        #endregion

        public void Dispose() => _serviceScope.Dispose();
    }

    public partial class DatabaseShaper
    {
        public const String DefaultClientId = "65a3b5c7d8e9f0123456789a";
        public const String DefaultClientSecret = "client_secret";
        public const String DefaultClientSecretHash = "$argon2id$v=19$m=65536,t=3,p=1$sVb/rkZ+lXmGUw7XBJOEmw$K5K7R8zOJLiVM5I6NKydkC4RNYLOjCR1PcLMimqZrWE";
        public const String DefaultClientRedirectUri = "_testing/callback";

        public const String DefaultRoleId = "6a3017fd8d5306e9a7c19119";
        public const String DefaultRoleName = "Client";

        public const String DefaultProtectedResourceId = "6a3019338d5306e9a7c19130";
        public const String DefaultProtectedResourceAudienceUri = "urn:gym:api";

        public const String DefaultScopeId = "6a301a2e8d5306e9a7c19140";

        public const String DefaultUserId = "6a1018950edd40bab32c5ff6";
        public const String DefaultUserPassword = "0cdSxf8N";
        public const String DefaultUserPasswordHash = "$argon2id$v=19$m=65536,t=3,p=1$noNLbw4SkUaAHOw5k3FLsw$AZtUtMu/yVKlsw9S3tq+ORvMivJzS7vCI7ta9lcwQhU";
        public const String DefaultUserUsername = "john_doe";

        public const String DefaultState = "test_state";
        public const String DefaultNonce = "test_nonce";

        public const String DefaultCodeVerifier = "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk";
        public const String DefaultCodeChallange = "E9Melhoa2OwvFrEMTJguCHaoeK1t8URWbuGJSstw-cM";
        public const String DefaultCodeChallangeMethod = "S256";

        public const String DefaultTelegramAssertion = "tgWebAppData=user%3D%257B%2522id%2522%253A5265776755%252C%2522first_name%2522%253A%2522Andrey%2522%252C%2522last_name%2522%253A%2522Kardashian%2522%252C%2522username%2522%253A%2522john_rogers_zhuzhma%2522%252C%2522language_code%2522%253A%2522en%2522%252C%2522allows_write_to_pm%2522%253Atrue%252C%2522photo_url%2522%253A%2522https%253A%255C%252F%255C%252Ft.me%255C%252Fi%255C%252Fuserpic%255C%252F320%255C%252FBoKwKY5IpDRWz6uIog74D57Ss61WeT4Jf5zTwZZS-1eHFwRJR56ePM-6SrkBKkaV.svg%2522%257D%26chat_instance%3D4756907096746764373%26chat_type%3Dprivate%26auth_date%3D1766844573%26signature%3DeNGfNiBcoU6lw7RV9SNMKxZNVfpltKsRWBG0MwDi1mzuLHnq0z_KkCRFnThl071xnR7cyWZ_IxcS2pz80NFHBQ%26hash%3D95553035c77fe4aa746125e758e8393d89e8fd15b18ec8de1471903d4ce1d044";
    }
}
