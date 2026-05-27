using Gym.AuthorizationServer.Services;

namespace Gym.AuthorizationServer.Tests.Servises
{
    public class RandomStringGeneratorTests
    {
        [Fact]
        public void Sussussful_Generates_Random_String()
        {
            //Given
            RandomBase64StringGenerator sut = new();

            //When
            var result = sut.Generate(8);

            //Then
            Assert.NotNull(result);
        }
    }
}
