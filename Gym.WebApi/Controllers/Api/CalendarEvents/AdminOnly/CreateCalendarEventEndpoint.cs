using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.CalendarEventApi.CreateCalendarEvent;
using Gym.Domain._Common;
using Gym.Domain._Shared.Errors;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.TrainingContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Admin))]
    public class CreateCalendarEventEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<CreateCalendarEventRequest>
        .WithActionResult<CreateCalendarEventResponse>
    {
        [HttpPost("api/admin-calendar-events")]
        public override async Task<ActionResult<CreateCalendarEventResponse>> HandleAsync(CreateCalendarEventRequest request, CancellationToken cancellationToken = default)
        {
            Result<CreateCalendarEventResult> createCalendarEventResult = await _mediator.Send(_mapper.Map<CreateCalendarEvent>(request), cancellationToken);

            if (createCalendarEventResult.Success)
            {
                return Accepted(
                    $"api/admin-calendar-events/{createCalendarEventResult.Data!.CalendarEventId}",
                    value: _mapper.Map<CreateCalendarEventResponse>(createCalendarEventResult.Data));
            }

            return createCalendarEventResult.Error switch
            {
                StartsAtValidationError 
                or EndsAtValidationError
                or TrainingPeriodValidationError
                or TrainingIdValidationError
                or CapacityValidationError
                or TrainingNotFoundError => this.BadRequestProblem(createCalendarEventResult.Error.GetErrorMessage()),

                _ => this.InternalErrorProblem(createCalendarEventResult.Error!.GetErrorMessage())
            };
        }

    }
}
