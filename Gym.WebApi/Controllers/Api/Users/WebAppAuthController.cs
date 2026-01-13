using AutoMapper;
using Gym.Application.Services.UserApi;
using Gym.Application.Services.UserApi.TelegramAuthentication;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users
{
    [Route("api/users")]
    [ApiController]
    [AllowAnonymous]
    public class WebAppAuthController(IMediator _mediator, IMapper _mapper, IAccessTokenGenerator _accessTokenGenerator) : ControllerBase
    {
        [HttpPost("web-app-auth")]
        public async Task<ActionResult<WebAppAuthResponse>> WebAppAuth(WebAppAuthRequest request)
        {
            UserDetails userDetails = await _mediator.Send(new AuthenticateUserCommand(request.initData));

            String accessToken = _accessTokenGenerator.Generate(userDetails);

            this.AppendCookiesWithAccessToken(accessToken);

            return Ok(_mapper.Map<WebAppAuthResponse>(userDetails));
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
