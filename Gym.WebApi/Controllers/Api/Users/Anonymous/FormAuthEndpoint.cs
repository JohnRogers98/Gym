using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.UserApi;
using Gym.Application.Services.UserApi.FormAuthentication;
using Gym.Domain._Common;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.FormAuthContext.Errors;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.Anonymous
{
    [ApiController]
    [AllowAnonymous]
    public class FormAuthEndpoint(IMediator _mediator, IMapper _mapper, IAccessTokenGenerator _accessTokenGenerator, IAccessCookieAppender _accessCookieAppender) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<AuthResponse>
    {
        [HttpPost("api/users/actions/form-auth")]
        public override async Task<ActionResult<AuthResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var credentials = Request.Headers.GetCredentialsFromBasicAuthorization();

            if(credentials.HasValue is false)
            {
                return this.BadRequestProblem("No authorization header in request.");
            }

            Result<AuthenticatedUserDetails> authenticatedUserDetailsResult = await _mediator.Send(
                new AuthenticateUser(credentials.Value.login, credentials.Value.password), cancellationToken);

            if (authenticatedUserDetailsResult.Success)
            {
                String accessToken = _accessTokenGenerator.Generate(authenticatedUserDetailsResult.Data!);
                _accessCookieAppender.AppendCookiesWithAccessToken(HttpContext, accessToken);

                return base.Ok(_mapper.Map<AuthResponse>(authenticatedUserDetailsResult.Data!));
            }

            return authenticatedUserDetailsResult.Error switch
            {
                LoginValidationError or SuchLoginNotExistsError => this.BadRequestProblem(authenticatedUserDetailsResult.Error!.GetErrorMessage()),
                _ => this.InternalErrorProblem(authenticatedUserDetailsResult.Error!.GetErrorMessage())
            };
        }
    }
    
}
