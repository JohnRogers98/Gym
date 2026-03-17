using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.Errors;
using Gym.Domain.TrainingContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    internal class CreateCalendarEventHandler(ICalendarEventRepository _calendarEventRepository, ITrainingRepository _trainingRepository) 
        : IRequestHandler<CreateCalendarEvent, Result<CreateCalendarEventResult>>
    {
        public async Task<Result<CreateCalendarEventResult>> Handle(CreateCalendarEvent request, CancellationToken cancellationToken)
        {
            var startsAtResult = StartsAt.From(request.Start);
            if (startsAtResult.Success is false)
                return Result<CreateCalendarEventResult>.Fail(startsAtResult.Error!);

            EndsAt? endsAt = null;
            if (request.End.HasValue)
            {
                var endsAtResult = EndsAt.From(request.End.Value);
                if (endsAtResult.Success is false)
                    return Result<CreateCalendarEventResult>.Fail(endsAtResult.Error!);
                endsAt = endsAtResult.Data;
            }

            var periodResult = TrainingPeriod.From(startsAtResult.Data!, endsAt);
            if (periodResult.Success is false)
                return Result<CreateCalendarEventResult>.Fail(periodResult.Error!);

            var trainingIdResult = TrainingId.From(request.TrainingId);
            if (!trainingIdResult.Success)
                return Result<CreateCalendarEventResult>.Fail(trainingIdResult.Error!);

            var isTrainingExists =  await _trainingRepository.ExistsAsync(trainingIdResult.Data!, cancellationToken);
            if (isTrainingExists is false)
                return Result<CreateCalendarEventResult>.Fail(TrainingNotFoundError.Create(trainingIdResult.Data!));

            Capacity capacity;
            if (request.MaxClientCount.HasValue)
            {
                var capacityResult = Capacity.From(request.MaxClientCount.Value);
                if(capacityResult.Success is false)
                    return Result<CreateCalendarEventResult>.Fail(capacityResult.Error!);
                capacity = capacityResult.Data!;
            }
            else
            {
                capacity = Capacity.Unlimited();
            }

            CalendarEvent calendarEvent = CalendarEvent.Create(
                _calendarEventRepository.NextIdentity(),
                periodResult.Data!,
                capacity,
                trainingIdResult.Data!,
                new HashSet<UserId>(),
                request.Instructors?.Select(instructor => InstructorId.From(instructor).Unwrap()));

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            return Result<CreateCalendarEventResult>.Ok(new CreateCalendarEventResult(calendarEvent.Id.Value));
        }
    }
}
