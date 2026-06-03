using System.IdentityModel.Tokens.Jwt;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        extension(ClaimsPrincipal principal)
        {
            public  String? GetNonce() => principal.FindFirst(JwtRegisteredClaimNames.Nonce)?.Value;

            public String? GetAtHash() => principal.FindFirst(JwtRegisteredClaimNames.AtHash)?.Value;

            public String? GetSub() => 
                principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
