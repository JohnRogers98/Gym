using Microsoft.IdentityModel.JsonWebTokens;

namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal principal)
    {
        public String GetRequiredUserId()
        {
            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (String.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("'Sub' not found in claims");

            return userId;
        }
    }
}
