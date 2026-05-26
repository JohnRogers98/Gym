namespace Gym.AuthorizationServer.Services
{
    public interface IGrantCodeGenerator
    {
        String GenerateGrantCode();
    }

    public class GrantCodeGenerator(IRandomStringGenerator _randomStringGenerator) : IGrantCodeGenerator
    {
        public String GenerateGrantCode() => _randomStringGenerator.Generate(32);
    }
}
