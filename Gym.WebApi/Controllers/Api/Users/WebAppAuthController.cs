using Gym.Application.Services.UserApi;
using Gym.Application.Services.UserApi.TelegramAuthentication;
using Gym.WebDto.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users
{
    [Route("api/users")]
    [ApiController]
    public class WebAppAuthController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("web-app-auth")]
        public async Task<IActionResult> WebAppAuth(WebAppAuthRequest request)
        {
            UserDetails userDetails = await _mediator.Send(new AuthenticateUserCommand(request.initData));
            return Ok();
        }
    }
}
