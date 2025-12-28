namespace Gym.Domain.UserAggregate.Authentication
{
    public record ValidatedTelegramUserInfo
    {
        public TelegramId Id { get; init; }
        
        private ValidatedTelegramUserInfo(TelegramId id)
        {
            Id = id;
        }

        public static ValidatedTelegramUserInfo From(TelegramId id) => new (id);
    }
}
