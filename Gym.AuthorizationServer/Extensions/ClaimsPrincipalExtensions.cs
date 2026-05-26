using System.Security.Claims;

namespace Gym.AuthorizationServer.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Boolean IsUserAuthenticated(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Identity is not null
            && claimsPrincipal.Identity.IsAuthenticated is true
            && claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value is not null;
    }
}
