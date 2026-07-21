using Gym.BFF.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Gym.BFF.Services.Session
{
    public interface ISetTokensToClientSideSessionService
    {
        Task HandleAsync(String accessToken, String? refreshToken, String? idToken = null);
    }

    public class SetTokensToClientSideSessionService(IHttpContextAccessor _httpContextAccessor) : ISetTokensToClientSideSessionService
    {
        public async Task HandleAsync(String accessToken, String? refreshToken, String? idToken = null)
        {
            if (_httpContextAccessor.HttpContext is null)
                return;

            var claims = new List<Claim>
            {
                new Claim(ExtendedClaimTypes.AccessToken, accessToken)
            };

            if(refreshToken is not null)
                claims.Add(new Claim(ExtendedClaimTypes.RefreshToken, refreshToken));

            if (idToken is not null)
                claims.Add(new Claim(ExtendedClaimTypes.IdToken, idToken));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                });
        }
    }
}
