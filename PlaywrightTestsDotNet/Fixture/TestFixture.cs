using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace PlaywrightTestsDotNet.Fixture;

public class TestFixture : IAsyncLifetime
{
    public TestFixture()
    {
        // Server = WireMockServer.Start(new WireMockServerSettings
        // {
        //     Port = 5009,
        //     StartAdminInterface = true,
        //     ReadStaticMappings = false
        // });
        //
        // Server.Given(Request.Create().WithPath("/data").UsingGet())
        //     .RespondWith(Response.Create()
        //         .WithHeader("Content-Type", "application/json")
        //         .WithBody("""
        //                   {
        //                     "items": [
        //                       { "name": "Catan", "yearPublished": 1995 },
        //                       { "name": "Terraforming Mars", "yearPublished": 2016 },
        //                       { "name": "Everdell", "yearPublished": 2018 }
        //                     ]
        //                   }
        //                   """)
        //         .WithStatusCode(200));
    }
    public WireMockServer Server { get; private set; }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        // Server.Stop();
        // Server.Dispose();
        return Task.CompletedTask;
    }
}