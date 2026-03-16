using AutoMapper;
using Gym.Application.Services.UserApi.TelegramAuthentication;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.Anonymous
{
    [Route("api/users/actions/web-app-auth")]
    [ApiController]
    [AllowAnonymous]
    public class WebAppAuthController(IMediator _mediator, IMapper _mapper, IAccessTokenGenerator _accessTokenGenerator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<WebAppAuthResponse>> WebAppAuth(WebAppAuthRequest request)
        {
            AuthenticatedUserDetails authenticatedUserDetails = await _mediator.Send(new AuthenticateUser(request.InitData));

            String accessToken = _accessTokenGenerator.Generate(authenticatedUserDetails);

            this.AppendCookiesWithAccessToken(accessToken);

            return Ok(_mapper.Map<WebAppAuthResponse>(authenticatedUserDetails));
        }

        private void AppendCookiesWithAccessToken(String accessToken)
        {
            HttpContext.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });
        }
    }
}
