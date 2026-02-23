using System.Security.Claims;

namespace Gym.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static String GetRequiredUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("UserId not found in claims");

            return userId;
        }

        public static String GetRequiredClientId(this ClaimsPrincipal principal)
        {
            var clientId = principal.FindFirst("ClientId")?.Value;

            if (string.IsNullOrEmpty(clientId))
                throw new UnauthorizedAccessException("ClientId not found in claims");

            return clientId;
        }
    }
}
