using Gym.BFF.Integration.Tests.Rsa;
using Gym.BFF.Options;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using WireMock.Server;

namespace Gym.BFF.Integration.Tests.Tests;

[Collection<BFFServerCollection>]
public class LoginTests(BFFServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
{
    [Fact]
    public async Task Successful_Login_Without_OIDC() 
    {
        var httpClient = Fixture.CreateClient();

        var urls = Fixture.Services.GetRequiredService<IOptions<UrlsOptions>>().Value;

        Fixture.AuthorizationServerMock.SetupExchageCodeToken(urls.AuthorizationServer.TokenEndpoint, "test_access_token", "test_refresh_token");

        var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
        var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
        String state = queryParams["state"]!;

        var callbackResponse = await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
        callbackResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Successful_Login_With_OIDC()
    {
        var httpClient = Fixture.CreateClient();

        var urls = Fixture.Services.GetRequiredService<IOptions<UrlsOptions>>().Value;
        var clientCredentials = Fixture.Services.GetRequiredService<IOptions<ClientCredentialsOptions>>().Value;
        
        var rsaKeyProvider = Fixture.Services.GetRequiredService<FakeRsaKeyProvider>();

        var signingCredentials = new SigningCredentials(
            Fixture.Services.GetRequiredService<FakeRsaSecutiryKey>().GetRsaSecurityKey(),
            SecurityAlgorithms.RsaSha256
        );

        RSAParameters publicParams = rsaKeyProvider.GetRsa().ExportParameters(true);
        var jwk = new Jwk
        {
            Algorithm = "RS256",
            KeyId = "gym_auth_key",
            KeyType = "RSA",
            PublicKeyUse = "sig",
            Modulus = Convert.ToBase64String(publicParams.Modulus!).ToUrlSafe(),
            Exponent = Convert.ToBase64String(publicParams.Exponent!).ToUrlSafe()
        };
        Fixture.AuthorizationServerMock.SetupJwks(urls.AuthorizationServer.Jwks, jwk);

        var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
        var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
        String state = queryParams["state"]!;
        String nonce = queryParams["nonce"]!;

        IdToken idToken = new()
        {
            Issuer = urls.AuthorizationServer.BaseUrl,
            Subject = "user",
            Audience = clientCredentials.ClientId,
            Expiration = DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds(),
            IssuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Nonce = nonce,
            AtHash = AtHashComputator.Compute("test_access_token")
        };
        var signedIdToken = idToken.Sign(signingCredentials);

        Fixture.AuthorizationServerMock.SetupExchageCodeToken(urls.AuthorizationServer.TokenEndpoint, "test_access_token", "test_refresh_token", signedIdToken);

        var callbackResponse = await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
        callbackResponse.EnsureSuccessStatusCode();
    }
}
