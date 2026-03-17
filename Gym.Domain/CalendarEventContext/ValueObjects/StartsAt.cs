using Gym.Domain._Common;

namespace Gym.Domain.CalendarEventContext.ValueObjects
{
    public record StartsAt
    {
        public DateTime Value { get; }

        private StartsAt(DateTime value) => Value = value;

        public static Result<StartsAt> From(DateTime value)
        {
            return Result<StartsAt>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
