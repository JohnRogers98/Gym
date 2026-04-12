using Gym.Domain._Common;

namespace Gym.Domain.TelegramAuthContext.Errors
{
    public class TelegramIdValidationError : DomainError
    {
        private TelegramIdValidationError() : base(nameof(TelegramIdValidationError)) { }

        public static TelegramIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Telegram id is invalid.";
    }
}
