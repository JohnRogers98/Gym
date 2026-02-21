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
    [Route("api/account/actions/get-history")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class GetAccountHistoryController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ListResponse<AccountHistoryDto>>> GetAccountHistory(GetAccountHistoryRequest request)
        {
            var getAccountHistory = _mapper.Map<GetAccountHistory>(request, opts =>
            {
                opts.Items[nameof(Application.Services.AccountApi.GetAccountHistory.GetAccountHistory.ClientId)] = User.GetRequiredClientId();
            });

            IEnumerable<EventProjection> eventProjections = await _mediator.Send(getAccountHistory);

            return base.Ok(
                new ListResponse<AccountHistoryDto>(
                    _mapper.Map<IEnumerable<AccountHistoryDto>>(eventProjections)
                )
            );
        }
    }
}
