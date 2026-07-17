using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.CalendarEventApi.CancelCalendarEvent;
using Gym.Domain._Common;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.TrainingContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly.CancelCalendarEventEndpoint;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Admin))]
    public class CancelCalendarEventEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<CancelCalendarEventContainer>
        .WithActionResult<CancelCalendarEventResponse>
    {
        [HttpPost("api/admin-calendar-events/{id}/actions/cancel")]
        public override async Task<ActionResult<CancelCalendarEventResponse>> HandleAsync(CancelCalendarEventContainer request, CancellationToken cancellationToken = default)
        {
            Result<CancelCalendarEventResult> cancelCalendarEventResult = await _mediator.Send(new CancelCalendarEvent(request.Id), cancellationToken);

            if(cancelCalendarEventResult.Success)
            {
                return base.Ok(_mapper.Map<CancelCalendarEventResponse>(cancelCalendarEventResult.Data));
            }

            return cancelCalendarEventResult.Error switch
            {
                TrainingNameValidationError or CalendarEventNotFoundError => this.BadRequestProblem(cancelCalendarEventResult.Error.GetErrorMessage()),
                EventStatusIncorrectForOperationError or CalendarEventBookingNotExistError => this.ConflictProblem(cancelCalendarEventResult.Error.GetErrorMessage()),
                _ => this.InternalErrorProblem(cancelCalendarEventResult.Error!.GetErrorMessage())
            };
        }

        public class CancelCalendarEventContainer
        {
            [FromRoute] public String Id { get; set; } = default!;
        }

    }
}
