using Gym.Domain._Common;

namespace Gym.Domain.UserContext.Errors
{
    public class TelegramUsernameValidationError : DomainError
    {
        private TelegramUsernameValidationError() : base(nameof(TelegramUsernameValidationError)) { }

        public static TelegramUsernameValidationError Create() => new();

        public override String GetErrorMessage() => $"Telegram username is invalid.";
    }
}
