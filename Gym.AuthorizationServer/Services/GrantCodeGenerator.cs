namespace Gym.AuthorizationServer.Services
{
    public interface IGrantCodeGenerator
    {
        String GenerateGrantCode();
    }

    public class GrantCodeGenerator(IRandomBase64StringGenerator _randomStringGenerator) : IGrantCodeGenerator
    {
        public String GenerateGrantCode() => _randomStringGenerator.Generate(32).ToUrlSafe();
    }
}
