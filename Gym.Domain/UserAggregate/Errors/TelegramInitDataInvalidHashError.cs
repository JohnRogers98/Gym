using Gym.Domain._Common;

namespace Gym.Domain.UserAggregate.Errors
{
    public class TelegramInitDataInvalidHashError : DomainError
    {
        private TelegramInitDataInvalidHashError() : base(nameof(TelegramInitDataInvalidHashError)) { }

        public static TelegramInitDataInvalidHashError Create() => new();

        public override String GetErrorMessage() => "Hash is not valid";
    }
}
