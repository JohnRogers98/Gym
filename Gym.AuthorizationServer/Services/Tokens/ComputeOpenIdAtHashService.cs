using Gym.OAuth.Extensions;

namespace Gym.AuthorizationServer.Services.Tokens
{
    public interface IComputeOpenIdAtHashService
    {
        String Compute(String accessToken);
    }

    public class ComputeOpenIdAtHashService : IComputeOpenIdAtHashService
    {
        public String Compute(String accessToken) => AtHashComputator.Compute(accessToken);
    }
}
