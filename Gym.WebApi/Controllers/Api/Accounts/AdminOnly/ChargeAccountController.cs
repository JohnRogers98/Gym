using AutoMapper;
using Gym.Application.Services.AccountApi;
using Gym.Application.Services.AccountApi.ChargeAccount;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Accounts.AdminOnly
{
    [Route("api/accounts/actions/charge")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ChargeAccountController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ChargeAccountResponse>> BookTrainingEvent(ChargeAccountRequest request)
        {
            AccountDetails accountDetails = await _mediator.Send(_mapper.Map<ChargeAccount>(request));

            return base.Ok(_mapper.Map<ChargeAccountResponse>(accountDetails));
        }
    }
}
