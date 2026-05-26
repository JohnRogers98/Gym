using Gym.AuthorizationServer.Services;

namespace Idp.Services
{
    public interface IRefreshTokenGenerator
    {
        String GenerateToken();
    }

    public class RefreshTokenGenerator(IRandomStringGenerator _randomStringGenerator) : IRefreshTokenGenerator
    {
        public String GenerateToken() => _randomStringGenerator.Generate(32);
    }
}
