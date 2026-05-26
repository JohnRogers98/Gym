using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Gym.AuthorizationServer.Integration.Tests.Antiforgery;

/// <summary>
/// A class containing valid antiforgery tokens for an ASP.NET Core application.
/// </summary>
public class AntiforgeryTokens
{
    /// <summary>
    /// Gets or sets the name of the cookie to use.
    /// </summary>
    [JsonProperty("cookieName")]
    [JsonPropertyName("cookieName")]
    public String CookieName { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the value to use for the antiforgery token HTTP cookie.
    /// </summary>
    [JsonProperty("cookieValue")]
    [JsonPropertyName("cookieValue")]
    public String CookieValue { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the name of the form parameter to use for the antiforgery token.
    /// </summary>
    [JsonProperty("formFieldName")]
    [JsonPropertyName("formFieldName")]
    public String FormFieldName { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the name of the HTTP request header to use for the antiforgery token.
    /// </summary>
    [JsonProperty("headerName")]
    [JsonPropertyName("headerName")]
    public String HeaderName { get; set; } = String.Empty;

    /// <summary>
    /// Gets or sets the value to use for the antiforgery token for forms and/or HTTP request headers.
    /// </summary>
    [JsonProperty("requestToken")]
    [JsonPropertyName("requestToken")]
    public String RequestToken { get; set; } = String.Empty;
}