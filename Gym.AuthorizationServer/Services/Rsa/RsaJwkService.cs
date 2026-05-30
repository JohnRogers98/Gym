using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaJwkService
    {
        Jwk GetJwk();
    }

    public class RsaJwkService : IRsaJwkService
    {
        private readonly Jwk _jwk;

        public RsaJwkService(IRsaKeyProvider rsaKeyService, IConfiguration configuration)
        {
            var keyId = configuration.GetRequiredConfiguration("Jwt:RsaKeyId");

            RSAParameters publicParams = rsaKeyService.GetRsa().ExportParameters(false);
            String n = Convert.ToBase64String(publicParams.Modulus!).ToUrlSafe();
            String e = Convert.ToBase64String(publicParams.Exponent!).ToUrlSafe();

            _jwk = new Jwk()
            {
                KeyType = "RSA",
                PublicKeyUse = "sig",
                Algorithm = "RS256",
                KeyId = keyId,
                Modulus = n,
                Exponent = e
            };
        }

        public Jwk GetJwk() => _jwk;
    }

    public class Jwk
    {
        [JsonPropertyName("kty")]
        public required String KeyType { get; set; }

        [JsonPropertyName("use")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? PublicKeyUse { get; set; }

        [JsonPropertyName("key_ops")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<String>? KeyOperations { get; set; }

        [JsonPropertyName("alg")]
        public required String Algorithm { get; set; }

        [JsonPropertyName("kid")]
        public required String KeyId { get; set; }

        [JsonPropertyName("n")]
        public required String Modulus { get; set; }

        [JsonPropertyName("e")]
        public required String Exponent { get; set; }
    }
}
