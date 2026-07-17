using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.AccountApi.ChargeAccount;
using Gym.Domain._Common;
using Gym.Domain._Shared.Errors;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.TrainingContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Gym.WebApi.Controllers.Api.Accounts.AdminOnly.ChargeAccountEndpoint;

namespace Gym.WebApi.Controllers.Api.Accounts.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Admin))]
    public class ChargeAccountEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<ChargeAccountContainer>
        .WithActionResult<ChargeAccountResponse>
    {
        [HttpPost("api/clients/{clientId}/account/actions/charge")]
        public override async Task<ActionResult<ChargeAccountResponse>> HandleAsync(ChargeAccountContainer request, CancellationToken cancellationToken = default)
        {
            var chargeAccount = _mapper.Map<ChargeAccount>(request.Body, opts =>
            {
                opts.Items[nameof(ChargeAccount.ClientId)] = request.ClientId;
            });

            Result<ChargeAccountResult> chargeAccountResult = await _mediator.Send(chargeAccount, cancellationToken);

            if(chargeAccountResult.Success)
            {
                return base.Ok(_mapper.Map<ChargeAccountResponse>(chargeAccountResult.Data));
            }

            return chargeAccountResult.Error switch
            {
                UserIdValidationError
                or CalendarEventIdValidationError
                or TrainingIdValidationError
                or AccountNotChargedError
                or ClientNotFoundError => this.BadRequestProblem(chargeAccountResult.Error.GetErrorMessage()),

                _ => this.InternalErrorProblem(chargeAccountResult.Error!.GetErrorMessage())
            };
        }

        public class ChargeAccountContainer
        {
            [FromRoute] public String ClientId { get; set; } = default!;

            [FromBody] public ChargeAccountRequest Body { get; set; } = default!;
        }

    }
}
