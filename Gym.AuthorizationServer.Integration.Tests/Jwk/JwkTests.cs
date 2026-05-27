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
            var client = Fixture.CreateClient();

            using var scope = Fixture.Services.CreateScope();
            var rsa = scope.ServiceProvider.GetRequiredService<IRsaKeyService>()
                .GetRsa();

            var data = Encoding.UTF8.GetBytes("Hello");

            Byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            #endregion

            #region Jwk response checking
            var jwksGetResponse = await client.GetAsync("/.well-known/jwks.json", TestContext.Current.CancellationToken);
            jwksGetResponse.EnsureSuccessStatusCode();

            var jwksObject = await jwksGetResponse.Content.ReadFromJsonAsync<JwkSet>(TestContext.Current.CancellationToken);
            Assert.NotNull(jwksObject);
            #endregion

            #region Signature checking
            Byte[] n = this.Base64UrlDecode(jwksObject.Jwks.First().Modulus);
            Byte[] e = this.Base64UrlDecode(jwksObject.Jwks.First().Exponent);

            using RSA rsaPublic = RSA.Create();
            rsaPublic.ImportParameters(new RSAParameters { Modulus = n, Exponent = e });
            Boolean result = rsaPublic.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            Assert.True(result);
            #endregion
        }

        private Byte[] Base64UrlDecode(String str)
        {
            String base64 = str.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

    }
}
