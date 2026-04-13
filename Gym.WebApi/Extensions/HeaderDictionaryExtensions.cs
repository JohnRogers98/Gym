using Microsoft.Extensions.Primitives;
using System.Text;

namespace Gym.WebApi.Extensions
{
    public static class HeaderDictionaryExtensions
    {
        public static (String login, String password)? GetCredentialsFromBasicAuthorization(this IHeaderDictionary headers)
        {
            var authHeader = headers.Authorization;

            if (StringValues.IsNullOrEmpty(authHeader) is true)
                return null;

            if (authHeader.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase) is false)
                return null;

            String encodedCredentials = authHeader.ToString().Substring(6).Trim();
            String decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            String[] credentials = decodedCredentials.Split(':', 2);

            if (credentials.Length == 2)
            {
                return (login: credentials[0], password: credentials[1]); 
            }

            return null;
        }

    }
}
