using Gym.BFF.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Gym.BFF.Services.Session
{
    public interface ISetSessionKeyToClientSession
    {
        Task HandleAsync(String clientSessionKey);
    }

    public class SetSessionKeyToClientSession(
        IHttpContextAccessor _httpContextAccessor,
        IOptions<Options.CookieOptions> _cookieOptions) : ISetSessionKeyToClientSession
    {
        public async Task HandleAsync(String clientSessionKey)
        {
            if (_httpContextAccessor.HttpContext is null || String.IsNullOrWhiteSpace(clientSessionKey))
                return;

            var claims = new List<Claim>
            {
                new Claim(ExtendedClaimTypes.ClientSessionKey, clientSessionKey)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow + _cookieOptions.Value.Ttl
                });
        }
    }
}
