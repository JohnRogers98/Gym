namespace Gym.BFF.Integration.Tests;

[Collection<BFFServerCollection>]
public abstract class IntegrationTest : IDisposable, IAsyncLifetime
{
    protected BFFServerFixture Fixture { get; }

    protected IntegrationTest(BFFServerFixture fixture, ITestOutputHelper outputHelper)
    {
        Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        Fixture.SetOutputHelper(outputHelper);
    }

    public void Dispose()
    {
        Fixture.SetOutputHelper(null);
    }

    public async ValueTask InitializeAsync()
    {
        Fixture.AuthorizationServerMock.Reset();
    }

    public async ValueTask DisposeAsync()
    {
        Fixture.AuthorizationServerMock.Reset();
    }
}