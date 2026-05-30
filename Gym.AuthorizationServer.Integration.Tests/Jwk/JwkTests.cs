using Gym.AuthorizationServer.Controllers.Api;
using Gym.AuthorizationServer.Services.Rsa;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace Gym.AuthorizationServer.Integration.Tests.Jwk
{
    [Collection<TestServerCollection>]
    public class JwkTests(TestServerFixture _fixture, ITestOutputHelper _outputHelper) : IntegrationTest(_fixture, _outputHelper)
    {
        [Fact]
        public async Task Verify_Rsa_Jwk_Signing()
        {
            #region Given
            var httpClient = Fixture.CreateClient();

            using var scope = Fixture.Services.CreateScope();
            var rsa = scope.ServiceProvider.GetRequiredService<IRsaKeyProvider>()
                .GetRsa();

            var data = Encoding.UTF8.GetBytes("Hello");

            Byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            #endregion

            #region Jwk response checking
            var jwksGetResponse = await httpClient.GetAsync("/.well-known/jwks.json", TestContext.Current.CancellationToken);
            jwksGetResponse.EnsureSuccessStatusCode();

            var jwksObject = await jwksGetResponse.Content.ReadFromJsonAsync<JwkSet>(TestContext.Current.CancellationToken);
            Assert.NotNull(jwksObject);
            #endregion

            #region Signature checking
            Byte[] n = jwksObject.Jwks.First().Modulus.Base64UrlDecode();
            Byte[] e = jwksObject.Jwks.First().Exponent.Base64UrlDecode();

            using RSA rsaPublic = RSA.Create();
            rsaPublic.ImportParameters(new RSAParameters { Modulus = n, Exponent = e });
            Boolean result = rsaPublic.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            Assert.True(result);
            #endregion
        }
    }
}
