using System.Text.Json.Serialization;

namespace Gym.OAuth.Extensions;

public class JwkSet
{
    [JsonPropertyName("keys")]
    public required List<Jwk> Jwks { get; set; }
}
