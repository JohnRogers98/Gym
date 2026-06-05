using Microsoft.AspNetCore.Mvc;

namespace Gym.OAuth.Extensions;

public class AuthorizeQuery
{
    [FromQuery(Name = "client_id")]
    public required String ClientId { get; set; }

    [FromQuery(Name = "response_type")]
    public required String ResponseType { get; set; }

    [FromQuery(Name = "redirect_uri")]
    public String? RedirectUri { get; set; }

    [FromQuery(Name = "scope")]
    public String? Scope { get; set; }

    [FromQuery(Name = "state")]
    public String? State { get; set; }

    [FromQuery(Name = "code_challenge")]
    public String? CodeChallenge { get; set; }

    [FromQuery(Name = "code_challenge_method")]
    public String? CodeChallengeMethod { get; set; }

    #region OIDC params
    [FromQuery(Name = "nonce")]
    public String? Nonce { get; set; }
    #endregion
}
