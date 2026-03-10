namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    public record AuthenticatedUserDetails(String UserId, String ClientId, String Role, Int64? TelegramId);
}
