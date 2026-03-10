using Gym.Application.Services.UserApi.TelegramAuthentication;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.Anonymous
{
    [Route("api/users/actions/admin-auth-mock")]
    [ApiController]
    [AllowAnonymous]
    public class AdminAuthMockController(IAccessTokenGenerator _accessTokenGenerator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AdminAuthMock()
        {
            AuthenticatedUserDetails authenticatedUserDetails = new AuthenticatedUserDetails("Undefined", "Undefined", "Admin", null);

            String accessToken = _accessTokenGenerator.Generate(authenticatedUserDetails);

            this.AppendCookiesWithAccessToken(accessToken);

            return Ok();
        }

        private void AppendCookiesWithAccessToken(String accessToken)
        {
            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
    }
}
