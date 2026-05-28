namespace Gym.AuthorizationServer.Services.Tokens
{
    public interface IRefreshTokenGenerator
    {
        String GenerateToken();
    }

    public class RefreshTokenGenerator(IRandomBase64StringGenerator _randomStringGenerator) : IRefreshTokenGenerator
    {
        public String GenerateToken() => _randomStringGenerator.Generate(32).ToUrlSafe();
    }
}
