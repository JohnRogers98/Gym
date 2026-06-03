using System.Text.Json.Serialization;

namespace Gym.BFF.Services.Jwks
{
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
