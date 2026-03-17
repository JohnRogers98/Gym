using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.Errors;

namespace Gym.Domain.CalendarEventContext.ValueObjects
{
    public record TrainingPeriod
    {
        public StartsAt StartsAt { get; }
        public EndsAt? EndsAt { get; }

        private TrainingPeriod(StartsAt startsAt, EndsAt? endsAt) 
            => (StartsAt, EndsAt) = (startsAt, endsAt);

        public static Result<TrainingPeriod> From(StartsAt startsAt, EndsAt? endsAt = null)
        {
            if (startsAt == null)
            {
                return Result<TrainingPeriod>.Fail(TrainingPeriodValidationError.Create());
            }
            if (endsAt is not null && startsAt.Value >= endsAt.Value)
            {
                return Result<TrainingPeriod>.Fail(TrainingPeriodValidationError.Create());
            }

            return Result<TrainingPeriod>.Ok(new(startsAt, endsAt));
        }

        public override String ToString() => $"{nameof(StartsAt)} - {StartsAt.Value}; {nameof(EndsAt)} - {EndsAt?.Value}";
    }
}
