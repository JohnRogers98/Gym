using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        extension(ClaimsPrincipal principal) 
        {
            public Boolean IsUserAuthenticated()
                => principal.Identity is not null
                && principal.Identity.IsAuthenticated is true
                && principal.FindFirst(ClaimTypes.NameIdentifier)?.Value is not null;

            public  String? GetSub() => principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            public String? GetScope() => principal.FindFirst("scope")?.Value;
        }
    }
}
