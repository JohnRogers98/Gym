using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.UserApi;
using Gym.Application.Services.UserApi.TelegramAuthentication;
using Gym.Domain._Common;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.TelegramAuthContext.Errors;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.Anonymous
{
    [ApiController]
    [AllowAnonymous]
    public class WebAppAuthEndpoint(IMediator _mediator, IMapper _mapper, IAccessTokenGenerator _accessTokenGenerator, IAccessCookieAppender _accessCookieAppender) : EndpointBaseAsync
        .WithRequest<WebAppAuthRequest>
        .WithActionResult<AuthResponse>
    {
        [HttpPost("api/users/actions/web-app-auth")]
        public override async Task<ActionResult<AuthResponse>> HandleAsync(WebAppAuthRequest request, CancellationToken cancellationToken = default)
        {
            Result<AuthenticatedUserDetails> authenticatedUserDetailsResult = await _mediator.Send(new AuthenticateUser(request.InitData), cancellationToken);

            if(authenticatedUserDetailsResult.Success)
            {
                String accessToken = _accessTokenGenerator.Generate(authenticatedUserDetailsResult.Data!);
                _accessCookieAppender.AppendCookiesWithAccessToken(HttpContext, accessToken);

                return base.Ok(_mapper.Map<AuthResponse>(authenticatedUserDetailsResult.Data!));
            }

            return authenticatedUserDetailsResult.Error switch
            {
                TelegramInitDataInvalidHashError telegramInitDataInvalidHash => this.BadRequestProblem(telegramInitDataInvalidHash.GetErrorMessage()),
                ClientNotFoundByUserIdError or _ => this.InternalErrorProblem(authenticatedUserDetailsResult.Error!.GetErrorMessage())
            };
        }

    }
}
