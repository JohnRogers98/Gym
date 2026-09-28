namespace Gym.BFF.Services.Session
{
    public interface ISessionKeyGenerator
    {
        String Generate();
    }

    public class SessionKeyGenerator : ISessionKeyGenerator
    {
        public String Generate() => Guid.NewGuid().ToString();
    }
}
