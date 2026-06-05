using System.Security.Cryptography;
using System.Text;

namespace Gym.OAuth.Extensions;

public static class AtHashComputator
{
    public static String Compute(String accessToken)
    {
        Byte[] asciiBytes = Encoding.ASCII.GetBytes(accessToken);
        Byte[] hash = SHA256.HashData(asciiBytes);
        Byte[] leftHalf = hash.Take(16).ToArray();
        return Convert.ToBase64String(leftHalf).ToUrlSafe();
    }
}
