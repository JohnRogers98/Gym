using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Admin.Application.Services.RoleApi.GetAllRoles;
using Gym.AuthorizationServer.Admin.Extensions;
using Gym.AuthorizationServer.Admin.Extensions.Mappings;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Admin.Controllers.Api.Roles
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ListUserRolesEndpoint(IMediator _mediator) : EndpointBaseAsync.WithoutRequest.WithActionResult<ListResponse<UserRoleDto>>
    {
        [HttpGet("api/user-roles")]
        public async override Task<ActionResult<ListResponse<UserRoleDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var userRoles = await _mediator.Send(new GetAllUserRoles(), cancellationToken);

            var response = new ListResponse<UserRoleDto>(userRoles.Select(userRoles => userRoles.ToResponseDto()));
            return base.Ok(response);
        }
    }
}
