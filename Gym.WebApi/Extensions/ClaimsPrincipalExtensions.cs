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
    }
}
