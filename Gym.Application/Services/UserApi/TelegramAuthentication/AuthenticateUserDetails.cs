namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    public record AuthenticateUserDetails(String Id, String Role, Int64? TelegramId);
}
