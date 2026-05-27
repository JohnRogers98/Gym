using Gym.AuthorizationServer.Services;

namespace Gym.AuthorizationServer.Tests.Servises
{
    public class ScopeCheckerTests
    {
        [Theory]
        [InlineData("scope_1", "scope_1", true)]
        [InlineData("scope_1 scope_2", "scope_1", true)]
        [InlineData("scope_1", "scope_1 scope_2", false)]
        [InlineData(null, null, true)]
        [InlineData(null, "scope_1", false)]
        [InlineData("scope_1", null, false)]
        public void Check_Success_When_Scopes_Is_The_Same(String? sourceScope, String? targetScope, Boolean expectedResult)
        {
            //Given
            ScopeChecker sut = new();

            //When
            var actualResult = sut.CheckScopes(sourceScope, targetScope);

            //Then
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
