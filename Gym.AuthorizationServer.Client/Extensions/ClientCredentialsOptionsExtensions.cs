using Gym.AuthorizationServer.Client.Options;
using System.Text;

namespace Gym.AuthorizationServer.Client;

public static class ClientCredentialsOptionsExtensions
{
    public static String GetCredentialsInBase64(this ClientCredentialsOptions clientCredentials)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientCredentials.ClientId}:{clientCredentials.ClientSecret}"));
}
