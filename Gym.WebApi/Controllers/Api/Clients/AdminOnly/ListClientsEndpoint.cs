using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.Clients;
using Gym.Application.Services.ClientApi.GetAllClients;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Clients.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ListClientsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<ClientDto>>
    {
        [HttpGet("api/admin-clients")]
        public override async Task<ActionResult<ListResponse<ClientDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<ClientProjection> clientProjections = await _mediator.Send(new GetAllClients(), cancellationToken);

            var response = new ListResponse<ClientDto>(_mapper.Map<IEnumerable<ClientDto>>(clientProjections));
            return base.Ok(response);
        }
    }
}
