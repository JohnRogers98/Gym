namespace Gym.AuthorizationServer.Integration.Tests;

[Collection<TestServerCollection>]
public abstract class IntegrationTest : IDisposable, IAsyncLifetime
{
    protected TestServerFixture Fixture { get; }

    protected IntegrationTest(TestServerFixture fixture, ITestOutputHelper outputHelper)
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
        await Fixture.ClearDatabaseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Fixture.ClearDatabaseAsync();
    }
}