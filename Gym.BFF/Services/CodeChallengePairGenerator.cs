using System.Security.Cryptography;
using System.Text;

namespace Gym.BFF.Services
{
    public interface ICodeChallengePairGenerator
    {
        CodeChallengePair Generate();
    }

    public class CodeChallengePairGenerator(IRandomBase64StringGenerator _randomBase64StringGenerator) : ICodeChallengePairGenerator
    {
        public CodeChallengePair Generate()
        {
            String codeVerifier = _randomBase64StringGenerator.Generate(byteLength: 64).ToUrlSafe();
            String codeChallenge = this.ComputeCodeChallenge(codeVerifier);
            
            return new(codeVerifier, codeChallenge, "S256");
        }

        /// <summary>
        /// Compute code challenge from code verifier.
        /// </summary>
        /// <param name="codeVerifier">Url-safe base64 code verifier.</param>
        /// <returns>Url-safe base64 code challenge.</returns>
        private String ComputeCodeChallenge(String codeVerifier) 
            => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier)))
            .ToUrlSafe();
    }

    public record CodeChallengePair(String CodeVerifier, String CodeChallenge, String CodeChallengeMethod);
}
