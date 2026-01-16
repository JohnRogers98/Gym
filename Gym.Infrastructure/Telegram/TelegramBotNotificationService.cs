using Gym.Domain._Shared;
using Gym.Domain.UserAggregate;
using Telegram.Bot;

namespace Gym.Infrastructure.Telegram
{
    internal class TelegramBotNotificationService(ITelegramBotClient _botClient, IUserQueryService _userQueryService) : INotificationService
    {
        /// <summary>
        /// Send message to bot. In telegram private chat ChatId is the same as TelegramId
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(UserId userId, String message, CancellationToken cancellationToken)
        {
            User? user = await _userQueryService.GetByIdAsync(userId, cancellationToken);
            if (user?.TelegramId is not null)
            {
                await _botClient.SendMessage(user.TelegramId.Value, message);
            } 
        }
    }
}
