using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Gym.WebApplication.Authentication
{
    public record AuthClaims
    {
        public required String UserId { get; init; }
        public required String Role { get; init; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var identity = new ClaimsIdentity(
            [
               new Claim(JwtRegisteredClaimNames.Sub, UserId), 
               new Claim(ClaimTypes.Role, Role)
            ], "OAuth2");

            return new ClaimsPrincipal(identity);
        }
    }
}
