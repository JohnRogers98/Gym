using System.Text.Json.Serialization;

namespace Gym.OAuth.Extensions;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public required String AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public required String TokenType { get; set; }

    [JsonPropertyName("refresh_token")]
    public String? RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public Int32? ExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public String? Scope { get; set; }

    [JsonPropertyName("id_token")]
    public String? IdToken { get; set; }
}
