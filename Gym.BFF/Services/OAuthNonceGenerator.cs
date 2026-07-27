namespace Gym.BFF.Services
{
    public interface IOAuthNonceGenerator
    {
        String Generate();
    }

    public class OAuthNonceGenerator : IOAuthNonceGenerator
    {
        public String Generate() => Guid.NewGuid().ToString();
    }
}
