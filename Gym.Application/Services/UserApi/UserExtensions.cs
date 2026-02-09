using Gym.Domain.UserContext;

namespace Gym.Application.Services.UserApi
{
    internal static class UserExtensions
    {
        public static UserDetails ToDetails(this User user)
            => new UserDetails(user.Id.Value, user.Role.ToString(), user.TelegramId?.Value);
    }
}
