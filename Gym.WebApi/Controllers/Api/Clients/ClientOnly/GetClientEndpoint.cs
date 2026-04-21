using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.Clients;
using Gym.Application.Services.ClientApi.GetClientById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Clients.ClientOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class GetClientEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<Response<ClientDto>>
    {
        [HttpGet("api/clients")]
        public override async Task<ActionResult<Response<ClientDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            GetClientById getClientById = new(User.GetRequiredUserId());
            ClientProjection? clientProjection = await _mediator.Send(getClientById, cancellationToken);

            if(clientProjection is null)
            {
                return this.InternalErrorProblem($"Client with id - {getClientById.ClientId} not fount.");
            }

            var response = new Response<ClientDto>(_mapper.Map<ClientDto>(clientProjection));
            return base.Ok(response);
        }
    }
}
