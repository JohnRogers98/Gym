using Gym.Domain._Shared;
using Gym.Domain.TelegramAuthContext;
using Gym.Domain.UserContext;
using Telegram.Bot;

namespace Gym.Infrastructure.Telegram
{
    internal class TelegramBotNotificationService(ITelegramBotClient _botClient, ITelegramAuthByUserIdFinder _telegramAuthByUserIdFinder) : INotificationService
    {
        /// <summary>
        /// Send message to bot. In telegram private chat ChatId is the same as TelegramId
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(UserId userId, String message, CancellationToken cancellationToken)
        {
            TelegramAuth? telegramAuth = await _telegramAuthByUserIdFinder.GetTelegramAuthByUserIdAsync(userId, cancellationToken);
            if (telegramAuth is not null)
            {
                await _botClient.SendMessage(telegramAuth.Id.Value, message);
            } 
        }
    }
}
