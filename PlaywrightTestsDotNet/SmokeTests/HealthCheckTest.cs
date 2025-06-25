using PlaywrightTestsDotNet.Fixture;

namespace PlaywrightTestsDotNet.SmokeTests;

public class HealthCheckTest : TestFixture
{
    private HttpClient _httpClient;
    private string _url;
    public override Task InitializeAsync()
    {
        _httpClient = new HttpClient();
        
        // TODO: Configure this in variable group in pipeline
        _url = "https://localhost:7212/api/HealthCheck";
        
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ApiHealthCheckTest()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, _url + "/api");
        
        var response = await _httpClient.SendAsync(message);
        Assert.True(response.IsSuccessStatusCode);
    }
    
    [Fact]
    public async Task DbHealthCheckTest()
    {
        var message = new HttpRequestMessage(HttpMethod.Get, _url + "/db");
        
        var response = await _httpClient.SendAsync(message);
        Assert.True(response.IsSuccessStatusCode);
    }
}