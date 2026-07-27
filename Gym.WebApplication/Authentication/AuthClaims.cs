using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Gym.WebApplication.Authentication
{
    public record AuthClaims
    {
        public required String UserId { get; init; }
        public required String Role { get; init; }
        public String? Name { get; init; }
        public String? AuthenticationMethod { get; init; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var identity = new ClaimsIdentity(
            [
               new Claim(JwtRegisteredClaimNames.Sub, UserId), 
               new Claim(ClaimTypes.Role, Role)
            ], "OAuth2");

            if (Name is not null)
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, Name));

            if (AuthenticationMethod is not null)
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Acr, AuthenticationMethod));

            return new ClaimsPrincipal(identity);
        }

        public static AuthClaims FromClaimsPrincipal(ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value 
                ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new InvalidOperationException("User ID claim not found");

            var role = principal.FindFirst(ClaimTypes.Role)?.Value
                ?? throw new InvalidOperationException("Role claim not found");

            var name = principal.FindFirst(JwtRegisteredClaimNames.Name)?.Value
                ?? principal.FindFirst(ClaimTypes.Name)?.Value;

            var acr = principal.FindFirst(JwtRegisteredClaimNames.Acr)?.Value;

            return new AuthClaims
            {
                UserId = userId,
                Role = role,
                Name = name,
                AuthenticationMethod = acr
            };
        }
    }
}
