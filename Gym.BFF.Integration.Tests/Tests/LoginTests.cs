using Gym.AuthorizationServer.Client.Options;
using Gym.BFF.Integration.Tests.Rsa;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
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

        var urls = Fixture.Services.GetRequiredService<AuthorizationServerOptions>();

        Fixture.AuthorizationServerMock.SetupExchageCodeToken(urls.TokenEndpoint, "test_access_token", "test_refresh_token");

        var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
        var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
        String state = queryParams["state"]!;

        var callbackResponse = await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Redirect, callbackResponse.StatusCode);
    }

    [Fact]
    public async Task Successful_Login_With_OIDC()
    {
        var httpClient = Fixture.CreateClient();

        var urls = Fixture.Services.GetRequiredService<AuthorizationServerOptions>();
        var clientCredentials = Fixture.Services.GetRequiredService<ClientCredentialsOptions>();
        
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
        Fixture.AuthorizationServerMock.SetupJwks(urls.JwksEndpoint, jwk);

        var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
        var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
        String state = queryParams["state"]!;
        String nonce = queryParams["nonce"]!;

        IdToken idToken = new()
        {
            Issuer = urls.BaseUrl,
            Subject = "user",
            Audience = clientCredentials.ClientId,
            Expiration = DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds(),
            IssuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Nonce = nonce,
            AtHash = AtHashComputator.Compute("test_access_token")
        };
        var signedIdToken = idToken.Sign(signingCredentials);

        Fixture.AuthorizationServerMock.SetupExchageCodeToken(urls.TokenEndpoint, "test_access_token", "test_refresh_token", signedIdToken);

        var callbackResponse = await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Redirect, callbackResponse.StatusCode);
    }
}
