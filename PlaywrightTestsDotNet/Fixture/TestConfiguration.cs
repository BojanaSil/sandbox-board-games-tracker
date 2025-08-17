namespace PlaywrightTestsDotNet.Fixture;

public static class TestConfiguration
{
    public static string GetBackendUrl()
    {
        // Single backend instance on port 5001
        return "http://localhost:5001";
    }
    
    public static string GetFrontendUrl()
    {
        // Frontend is always on the same port
        return "http://localhost:4300";
    }
    
    public static string GetApiUrl(string endpoint)
    {
        return $"{GetBackendUrl()}/api/{endpoint}";
    }
}