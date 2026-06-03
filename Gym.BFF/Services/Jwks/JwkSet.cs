using System.Text.Json.Serialization;

namespace Gym.BFF.Services.Jwks
{
    public class JwkSet
    {
        [JsonPropertyName("keys")]
        public required List<Jwk> Jwks { get; set; }
    }

}
