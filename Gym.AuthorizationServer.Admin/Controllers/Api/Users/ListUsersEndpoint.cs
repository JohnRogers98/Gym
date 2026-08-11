using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Admin.Application.Services.UserApi.GetAllUsers;
using Gym.AuthorizationServer.Admin.Extensions;
using Gym.AuthorizationServer.Admin.Extensions.Mappings;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Admin.Controllers.Api.Users
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ListUsersEndpoint(IMediator _mediator) : EndpointBaseAsync.WithoutRequest.WithActionResult<ListResponse<UserDto>>
    {
        [HttpGet("api/users")]
        public async override Task<ActionResult<ListResponse<UserDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var users = await _mediator.Send(new GetAllUsers(), cancellationToken);

            var response = new ListResponse<UserDto>(users.Select(userRoles => userRoles.ToResponseDto()));
            return base.Ok(response);
        }
    }
}
