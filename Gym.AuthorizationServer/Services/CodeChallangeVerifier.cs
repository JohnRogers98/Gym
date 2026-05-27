using System.Security.Cryptography;
using System.Text;

namespace Gym.AuthorizationServer.Services
{
    public interface ICodeChallangeVerifier
    {
        Boolean Verify(String codeVerifier, String codeChallange, String codeChallangeMethod);
    }

    public class CodeChallangeVerifier : ICodeChallangeVerifier
    {
        public Boolean Verify(String codeVerifier, String codeChallange, String codeChallangeMethod)
        {
            if(codeChallangeMethod == "S256")
            {
                byte[] computedCodeVerifierHash = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
                String recomputedCodeChallange = Convert.ToBase64String(computedCodeVerifierHash).ToUrlSafe();
                return codeChallange == recomputedCodeChallange;
            }

            return false;
        }
    }
}
