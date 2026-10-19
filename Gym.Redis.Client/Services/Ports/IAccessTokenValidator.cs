namespace Gym.Redis.Client.Services.Ports
{
    public interface IAccessTokenValidator
    {
        Boolean Validate(String accessToken);
    }
}
