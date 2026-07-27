using System.IdentityModel.Tokens.Jwt;

namespace System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static String? GetSub(this ClaimsPrincipal principal) 
        => principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value 
        ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
