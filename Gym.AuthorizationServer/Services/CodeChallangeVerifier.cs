using System.Security.Cryptography;
using System.Text;

namespace Gym.AuthorizationServer.Services
{
    public interface ICodeChallangeVerifier
    {
        Boolean Verify(String codeVerifier, String codeChallange, String? codeChallangeMethod);
    }

    public class CodeChallangeVerifier : ICodeChallangeVerifier
    {
        public Boolean Verify(String codeVerifier, String codeChallange, String? codeChallangeMethod)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));

            String base64CodeChallange = Convert.ToBase64String(hash);
            String urlSafeCodeChallange = base64CodeChallange
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            return codeChallange == urlSafeCodeChallange;
        }
    }
}
