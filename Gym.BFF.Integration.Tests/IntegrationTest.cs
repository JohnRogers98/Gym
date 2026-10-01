using Gym.AuthorizationServer.Client.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WireMock.Server;

namespace Gym.BFF.Integration.Tests;

[Collection<BFFServerCollection>]
public abstract class IntegrationTest : IDisposable, IAsyncLifetime
{
    protected BFFServerFixture Fixture { get; }

    protected IntegrationTest(BFFServerFixture fixture, ITestOutputHelper outputHelper)
    {
        Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        Fixture.SetOutputHelper(outputHelper);
    }

    public void Dispose()
    {
        Fixture.SetOutputHelper(null);
    }

    public async ValueTask InitializeAsync()
    {
        Fixture.AuthorizationServerMock.Reset();
    }

    public async ValueTask DisposeAsync()
    {
        Fixture.AuthorizationServerMock.Reset();
    }
}

public class ExtendedIntegrationTest : IntegrationTest
{
    public ExtendedIntegrationTest(BFFServerFixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper) { }

    public async Task MakeLoginCallToSetupSesionCookieAsync(HttpClient httpClient)
    {
        await this.MakeLoginCallToSetupSesionCookieAsync(httpClient, this.CreateJwtToken(), Guid.NewGuid().ToString());
    }

    public async Task MakeLoginCallToSetupSesionCookieAsync(HttpClient httpClient, String accessToken, String refreshToken)
    {
        Fixture.AuthorizationServerMock.SetupExchangeCodeToken(this.GetTokenUrl(), accessToken, refreshToken);

        var loginResponse = await httpClient.GetAsync("/login", TestContext.Current.CancellationToken);
        var queryParams = QueryHelpers.ParseQuery(loginResponse.Headers.Location!.Query);
        String state = queryParams["state"]!;

        await httpClient.GetAsync($"/callback?code=test_code&state={state}", TestContext.Current.CancellationToken);
    }

    public String GetTokenUrl() => Fixture.Services.GetRequiredOption<AuthorizationServerOptions>().TokenEndpoint;

    public String CreateJwtToken(Boolean isExpired = false)
    {
        var claimsIdentity = new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Iss, "test_issuer"),
                new Claim(JwtRegisteredClaimNames.Aud, "test_audience"),
                new Claim(JwtRegisteredClaimNames.Sub, "test_subject")
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = isExpired ? DateTime.UtcNow.AddMinutes(-5) : DateTime.UtcNow.AddMinutes(5)
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}