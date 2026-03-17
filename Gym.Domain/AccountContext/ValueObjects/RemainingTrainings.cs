using Gym.Domain._Common;
using Gym.Domain.AccountContext.Errors;

namespace Gym.Domain.AccountContext.ValueObjects
{
    public record RemainingTrainings
    {
        public Int32 Value { get; }

        private RemainingTrainings(Int32 value) => Value = value;

        public static Result<RemainingTrainings> From(Int32 value)
        {
            if (value < 0)
            {
                return Result<RemainingTrainings>.Fail(RemainingTrainingsValidationError.Create());
            }

            return Result<RemainingTrainings>.Ok(new(value));
        }

        public Boolean CanBook() => Value > 0;

        public Result<RemainingTrainings> Decrement(Int32 byCount = 1) => From(Value - byCount);

        public Result<RemainingTrainings> Increment(Int32 byCount = 1) => From(Value + byCount);

        public override String ToString() => Value.ToString();
    }
}
