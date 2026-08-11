namespace Gym.AuthorizationServer.Tests
{
    public class ExtensionTests
    {
        [Theory]
        [InlineData("a+b/ab=", "a-b_ab")]
        public void To_Url_Safe_Returns_Correct_String(String urlUnsafeStr, String expectedResult)
        {
            Assert.Equal(expectedResult, urlUnsafeStr.ToUrlSafe());
        }
    }
}
