using System.Security.Cryptography;

namespace Gym.BFF.Services
{
    public interface IRandomBase64StringGenerator
    {
        String Generate(Int32 byteLength);
    }

    public class RandomBase64StringGenerator : IRandomBase64StringGenerator
    {
        public String Generate(Int32 byteLength)
        {
            Byte[] randomBytes = new Byte[byteLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
    }
}
