using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Services
{
    public interface IRandomStringGenerator
    {
        String Generate(Int32 byteLength);
    }

    public class RandomStringGenerator : IRandomStringGenerator
    {
        public String Generate(Int32 byteLength)
        {
            Byte[] randomBytes = new Byte[byteLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            String base64 = Convert.ToBase64String(randomBytes);

            String urlSafeBase64 = base64
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            return urlSafeBase64;
        }
    }
}
