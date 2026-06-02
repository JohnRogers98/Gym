namespace Gym.BFF.Services.Token
{
    public class OAuthTokenRequest
    {
        public String? ClientId { get; set; }
        public String? ClientSecret { get; set; }
        public required String RedirectUri { get; set; }
        public required String GrantType { get; set; }
        public String? Code { get; set; }
        public String? Scope { get; set; }
        public String? RefreshToken { get; set; }
        public String? Assertion { get; set; }
        public String? CodeVerifier { get; set; }
    }
}
