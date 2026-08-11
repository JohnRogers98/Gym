using Gym.Domain._Common;

namespace Gym.Domain._Shared.Errors
{
    public class StartsAtValidationError : DomainError
    {
        private StartsAtValidationError() : base(nameof(StartsAtValidationError)) { }

        public static StartsAtValidationError Create() => new();

        public override String GetErrorMessage() => $"Starts at is invalid.";
    }
}
