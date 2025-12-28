namespace Gym.Domain.Users.Authentication
{
    public record ValidatedTelegramUserInfo
    {
        public TelegramUserId Id { get; init; }
        
        private ValidatedTelegramUserInfo(TelegramUserId id)
        {
            Id = id;
        }

        public static ValidatedTelegramUserInfo From(TelegramUserId id) => new (id);
    }
}
