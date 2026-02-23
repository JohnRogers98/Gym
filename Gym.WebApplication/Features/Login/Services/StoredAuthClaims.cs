using System.Security.Claims;

namespace Gym.WebApplication.Features.Login.Services
{
    public record StoredAuthClaims
    {
        public required String UserId { get; init; }
        public required String Role { get; init; }

        public ClaimsPrincipal ToClaimsPrincipal()
        {
            var identity = new ClaimsIdentity(
            [
               new Claim(ClaimTypes.Role, Role),
            ], "WebApp Authentication");

            return new ClaimsPrincipal(identity);
        }
    }
}
