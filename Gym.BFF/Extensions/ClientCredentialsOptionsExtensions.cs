using System.Text;

namespace Gym.BFF.Options;

public static class ClientCredentialsOptionsExtensions
{
    public static String GetCredentialsInBase64(this ClientCredentialsOptions clientCredentials)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientCredentials.ClientId}:{clientCredentials.ClientSecret}"));
}
