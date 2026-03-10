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
    [Route("api/admin-clients")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ListClientsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ListResponse<ClientDto>>> ListClients()
        {
            IEnumerable<ClientProjection> clientProjections = await _mediator.Send(new GetAllClients());

            var response = new ListResponse<ClientDto>(_mapper.Map<IEnumerable<ClientDto>>(clientProjections));
            return base.Ok(response);
        }
    }
}
