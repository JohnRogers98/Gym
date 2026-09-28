namespace Gym.Infrastructure.Notifications.Telegram
{
    internal record TelegramBotToken
    {
        public String Value { get; init; }

        private TelegramBotToken(String value) { Value = value; }

        public static TelegramBotToken From(String value) => new TelegramBotToken(value);
    }
}
