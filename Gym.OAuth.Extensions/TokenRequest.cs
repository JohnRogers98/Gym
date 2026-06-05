using Microsoft.AspNetCore.Mvc;

namespace Gym.OAuth.Extensions;

public class TokenRequest
{
    [FromForm(Name = "client_id")]
    public String? ClientId { get; set; }

    [FromForm(Name = "client_secret")]
    public String? ClientSecret { get; set; }

    [FromForm(Name = "redirect_uri")]
    public required String RedirectUri { get; set; }

    [FromForm(Name = "grant_type")]
    public required String GrantType { get; set; }

    [FromForm(Name = "code")]
    public String? Code { get; set; }

    [FromForm(Name = "scope")]
    public String? Scope { get; set; }

    [FromForm(Name = "refresh_token")]
    public String? RefreshToken { get; set; }

    [FromForm(Name = "assertion")]
    public String? Assertion { get; set; }

    [FromForm(Name = "code_verifier")]
    public String? CodeVerifier { get; set; }
}
