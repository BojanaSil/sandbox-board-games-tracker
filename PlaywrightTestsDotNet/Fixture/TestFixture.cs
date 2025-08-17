namespace PlaywrightTestsDotNet.Fixture;

public class TestFixture : IAsyncLifetime
{
    public TestFixture()
    {
        // WireMock is now managed by Docker Compose for acceptance tests
        // No need to start it programmatically
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}