using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.EventStore;
using Gym.Application.Services.AccountApi.GetAccountHistory;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Accounts.AuthenticatedOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class GetAccountHistoryEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetAccountHistoryRequest>
        .WithActionResult<ListResponse<AccountHistoryDto>>
    {
        [HttpPost("api/account/actions/get-history")]
        public override async Task<ActionResult<ListResponse<AccountHistoryDto>>> HandleAsync(GetAccountHistoryRequest request, CancellationToken cancellationToken = default)
        {
            GetAccountHistory getAccountHistory = new(User.GetRequiredUserId());

            IEnumerable<EventProjection> eventProjections = await _mediator.Send(getAccountHistory, cancellationToken);

            var response = new ListResponse<AccountHistoryDto>(_mapper.Map<IEnumerable<AccountHistoryDto>>(eventProjections));
            return base.Ok(response);
        }

    }
}
