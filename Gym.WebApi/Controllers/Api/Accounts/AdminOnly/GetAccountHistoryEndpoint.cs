using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.EventStore;
using Gym.Application.Services.AccountApi.GetAccountHistoryByUserId;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Gym.WebApi.Controllers.Api.Accounts.AdminOnly.GetAccountHistoryEndpoint;

namespace Gym.WebApi.Controllers.Api.Accounts.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class GetAccountHistoryEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetAccountHistoryContainer>
        .WithActionResult<ListResponse<AccountHistoryDto>>
    {
        [HttpPost("api/clients/{clientId}/account/actions/get-history")]
        public override async Task<ActionResult<ListResponse<AccountHistoryDto>>> HandleAsync(GetAccountHistoryContainer request, CancellationToken cancellationToken = default)
        {
            GetAccountHistoryByClientId getAccountHistoryByClientId = new(request.ClientId);

            IEnumerable<EventProjection> eventProjections = await _mediator.Send(getAccountHistoryByClientId, cancellationToken);

            var result = new ListResponse<AccountHistoryDto>(_mapper.Map<IEnumerable<AccountHistoryDto>>(eventProjections));
            return base.Ok(result);
        }

        public class GetAccountHistoryContainer
        {
            [FromRoute] public String ClientId { get; set; } = default!;

            [FromBody] public GetAccountHistoryRequest Body { get; set; } = default!;
        }

    }
}
