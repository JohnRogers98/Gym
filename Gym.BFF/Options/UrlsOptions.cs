using System.ComponentModel.DataAnnotations;

namespace Gym.BFF.Options
{
    public class UrlsOptions
    {
        public const String SectionName = "Urls";

        public AuthorizationServerOptions AuthorizationServer { get; set; } = new();
    }

    public class AuthorizationServerOptions
    {
        [Required]
        [Url]
        public String BaseUrl { get; set; } = default!;

        public String AuthorizeEndpoint { get; set; } = "/authorize";
        public String TokenEndpoint { get; set; } = "/token";
        public String UserInfoEndpoint { get; set; } = "/userinfo";
        public String Jwks { get; set; } = "/.well-known/jwks.json";

        [Required]
        public String Kid { get; set; } = default!;

        public String FullAuthorizeUrl => $"{BaseUrl}{AuthorizeEndpoint}";
        public String FullTokenUrl => $"{BaseUrl}{TokenEndpoint}";
        public String FullUserInfoUrl => $"{BaseUrl}{UserInfoEndpoint}";
        public String FullEndSessionUrl => $"{BaseUrl}{Jwks}";
    }
}
