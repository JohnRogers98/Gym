namespace Gym.AuthorizationServer.Services
{
    public interface IRequestIdGenerator
    {
        String Generate();
    }

    public class RequestIdGenerator : IRequestIdGenerator
    {
        public String Generate() => Guid.NewGuid().ToString();
    }
}
