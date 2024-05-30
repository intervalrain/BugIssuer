namespace BugIssuer.Test.IntegrationTests.Common;

public class AppHttpClient
{
    private readonly HttpClient _httpClient;

    public AppHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}

