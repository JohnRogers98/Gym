using AutoMapper;
using Gym.Application.Services.AccountApi.ChargeAccount;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Accounts.AdminOnly
{
    [Route("api/clients/{clientId}/account/actions/charge")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ChargeAccountController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ChargeAccountResponse>> BookTrainingEvent(String clientId, ChargeAccountRequest request)
        {
            var chargeAccount = _mapper.Map<ChargeAccount>(request, opts =>
            {
                opts.Items[nameof(ChargeAccount.ClientId)] = clientId;
            });

            ChargeAccountResult chargeAccountResult = await _mediator.Send(chargeAccount);

            return base.Ok(_mapper.Map<ChargeAccountResponse>(chargeAccountResult));
        }
    }
}
