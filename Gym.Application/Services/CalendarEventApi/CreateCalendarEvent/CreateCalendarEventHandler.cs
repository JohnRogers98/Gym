using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.Errors;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.Errors;
using Gym.Domain.TrainingContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    internal class CreateCalendarEventHandler(
        ICalendarEventRepository _calendarEventRepository,
        ITrainingRepository _trainingRepository,
        IInstructorRepository _instructorRepository,
        IPollRepository _pollRepository) 
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

            var isTrainingExists = await _trainingRepository.ExistsAsync(trainingIdResult.Data!, cancellationToken);
            if (isTrainingExists is false)
                return Result<CreateCalendarEventResult>.Fail(TrainingNotFoundError.Create(trainingIdResult.Data!));

            List<InstructorId>? instructorIds = null;
            if(request.Instructors is not null)
            {
                instructorIds = new List<InstructorId>();

                foreach (var anInstructor in request.Instructors)
                {
                    var instructorIdResult = InstructorId.From(anInstructor);
                    if (instructorIdResult.Success is false)
                        return Result<CreateCalendarEventResult>.Fail(instructorIdResult.Error!);

                    Boolean instructorExist = await _instructorRepository.ExistsAsync(instructorIdResult.Data!, cancellationToken);
                    if(instructorExist is false)
                        return Result<CreateCalendarEventResult>.Fail(InstructorNotFoundError.Create(instructorIdResult.Data!));

                    instructorIds.Add(instructorIdResult.Data!);
                }
            }

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

            Poll? poll = null;
            if (request.Poll is not null)
            {
                var createPollResult = await this.CreatePollAsync(request.Poll, cancellationToken);
                if (createPollResult.Success is false)
                    return Result<CreateCalendarEventResult>.Fail(createPollResult.Error!);

                poll = createPollResult.Data!;
            }

            CalendarEvent calendarEvent = CalendarEvent.Create(
                _calendarEventRepository.NextIdentity(),
                periodResult.Data!,
                capacity,
                trainingIdResult.Data!,
                new HashSet<UserId>(),
                instructorIds,
                poll?.Id);

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            return Result<CreateCalendarEventResult>.Ok(new CreateCalendarEventResult(calendarEvent.Id.Value));
        }

        private async Task<Result<Poll>> CreatePollAsync(CalendarEventPoll calendarEventPoll, CancellationToken cancellationToken)
        {
            var pollTitleResult = PollTitle.From(calendarEventPoll.Title);
            if (pollTitleResult.Success is false)
                return Result<Poll>.Fail(pollTitleResult.Error!);

            List<ChoiceText> choiceTexts = new();
            foreach (var aChoiceVariant in calendarEventPoll.ChoiceVariants)
            {
                var choiceTextResult = ChoiceText.From(aChoiceVariant);
                if (choiceTextResult.Success is false)
                    return Result<Poll>.Fail(choiceTextResult.Error!);

                choiceTexts.Add(choiceTextResult.Data!);
            }

            var createPollResult = Poll.Create(
                _pollRepository.NextIdentity(),
                pollTitleResult.Data!,
                IsResponseRequired.From(calendarEventPoll.IsResponseRequired),
                CanAcceptManyChoices.From(calendarEventPoll.CanAcceptMany),
                choiceTexts);

            if (createPollResult.Success is false)
                return Result<Poll>.Fail(createPollResult.Error!);

            await _pollRepository.SaveAsync(createPollResult.Data!, cancellationToken);

            return Result<Poll>.Ok(createPollResult.Data!);
        }

    }
}
