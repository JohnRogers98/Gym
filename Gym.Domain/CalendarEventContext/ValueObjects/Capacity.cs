using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.Errors;

namespace Gym.Domain.CalendarEventContext.ValueObjects
{
    public record Capacity
    {
        public Int32 Value { get; }

        private Capacity(Int32 value) => Value = value;

        public static Result<Capacity> From(Int32 value)
        {
            if(value < 0)
            {
                return Result<Capacity>.Fail(CapacityValidationError.Create());
            }

            return Result<Capacity>.Ok(new(value));
        }

        public static Capacity Unlimited()
        {
            return new (Int32.MaxValue);
        }

        public override String ToString() => Value.ToString();
    }
}
