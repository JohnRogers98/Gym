using System.Text.Json.Serialization;

namespace Gym.OAuth.Extensions;

public class UserInfo
{
    [JsonPropertyName("sub")]
    public required String Subject { get; set; }

    #region profile
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public String? Name { get; set; }

    [JsonPropertyName("given_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public String? GivenName { get; set; }

    [JsonPropertyName("family_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public String? FamilyName { get; set; }
    #endregion

    #region Extensions
    [JsonPropertyName("role")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public String? Role { get; set; }
    #endregion
}
