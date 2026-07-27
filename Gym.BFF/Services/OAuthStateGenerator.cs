namespace Gym.BFF.Services
{
    public interface IOAuthStateGenerator
    {
        String Generate();
    }

    public class OAuthStateGenerator : IOAuthStateGenerator
    {
        public String Generate() => Guid.NewGuid().ToString();
    }
}
