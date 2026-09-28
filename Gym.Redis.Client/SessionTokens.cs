namespace Gym.Redis.Client
{
    public class SessionTokens
    {
        public required String AccessToken { get; set; }
        public String? RefreshToken { get; set; }
        public String? IdToken { get; set; }
    }
}
