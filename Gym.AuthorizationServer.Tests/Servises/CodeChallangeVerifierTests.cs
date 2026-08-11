using Gym.AuthorizationServer.Services;
using System.Security.Cryptography;
using System.Text;

namespace Gym.AuthorizationServer.Tests.Servises
{
    public class CodeChallangeVerifierTests
    {
        [Fact]
        public void Verify_That_Are_Matched_By_S256_Method()
        {
            //Given
            CodeChallangeVerifier sut = new();
            String codeVerifier = "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk";
            Byte[] verifierBytes = Encoding.UTF8.GetBytes(codeVerifier);

            using SHA256 sha256 = SHA256.Create();    
            Byte[] codeVerifierHashBytes = sha256.ComputeHash(verifierBytes);    
            String codeChallenge = this.ToUrlSafe(Convert.ToBase64String(codeVerifierHashBytes));

            //When
            var result = sut.Verify(codeVerifier, codeChallenge, "S256");

            //Then
            Assert.True(result);
        }

        [Fact]
        public void Verify_That_Are_NOT_Matched_By_S256_Method()
        {
            //Given
            CodeChallangeVerifier sut = new();
            String codeVerifier = "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk";
            Byte[] verifierBytes = Encoding.UTF8.GetBytes(codeVerifier);

            using SHA256 sha256 = SHA256.Create();
            Byte[] codeVerifierHashBytes = sha256.ComputeHash(verifierBytes);
            String codeChallenge = this.ToUrlSafe(Convert.ToBase64String(codeVerifierHashBytes));

            //When
            var result = sut.Verify($"{codeVerifier}a", codeChallenge, "S256");

            //Then
            Assert.False(result);
        }

        [Fact]
        public void Verify_That_Are_NOT_Matched_When_Another_Algoriphm()
        {
            //Given
            CodeChallangeVerifier sut = new();
            String codeVerifier = "dBjftJeZ4CVP-mB92K27uhbUJU1p1r_wW1gFWFOEjXk";
            Byte[] verifierBytes = Encoding.UTF8.GetBytes(codeVerifier);

            using SHA256 sha256 = SHA256.Create();
            Byte[] codeVerifierHashBytes = sha256.ComputeHash(verifierBytes);
            String codeChallenge = this.ToUrlSafe(Convert.ToBase64String(codeVerifierHashBytes));

            //When
            var result = sut.Verify(codeVerifier, codeChallenge, "plain");

            //Then
            Assert.False(result);
        }

        private String ToUrlSafe(String str) => str.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
}
