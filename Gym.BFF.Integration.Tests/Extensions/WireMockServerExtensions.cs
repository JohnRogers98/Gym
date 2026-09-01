using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace WireMock.Server
{
    public static class WireMockServerExtensions
    {
        extension(WireMockServer server)
        {
            public void SetupJwks(String path, Jwk jwk)
            {
                JwkSet jwks = new JwkSet
                {
                    Jwks = [jwk]
                };
                var jwksJson = JsonSerializer.Serialize(jwks);

                server
                    .Given(Request.Create().WithPath(new PathString("/" + path)).UsingGet())
                    .RespondWith(
                        Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(jwksJson)
                    );
            }

            public void SetupExchageCodeToken(String path, String accessToken, String refreshToken, String? idToken = null)
            {
                var tokenResponse = new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    IdToken = idToken,
                    ExpiresIn = 3600,
                    TokenType = "Bearer"
                };
                var tokenResponseJson = JsonSerializer.Serialize(tokenResponse);

                server
                    .Given(Request.Create().WithPath(new PathString("/" + path)))
                    .RespondWith(
                        Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(tokenResponseJson)
                    );
            }
        }
    }
}
