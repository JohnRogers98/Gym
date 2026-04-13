using Ardalis.ApiEndpoints;
using Gym.Application.Services.UserApi;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.Anonymous
{
    [ApiController]
    [AllowAnonymous]
    public class AdminAuthMockEndpoint(IAccessTokenGenerator _accessTokenGenerator, IAccessCookieAppender _accessCookieAppender) : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        [HttpPost("api/users/actions/admin-auth-mock")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            AuthenticatedUserDetails authenticatedUserDetails = new AuthenticatedUserDetails("Undefined", "Admin");

            String accessToken = _accessTokenGenerator.Generate(authenticatedUserDetails);
            _accessCookieAppender.AppendCookiesWithAccessToken(HttpContext, accessToken);

            return base.Ok();
        }

    }
}
