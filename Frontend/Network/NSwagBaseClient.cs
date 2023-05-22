using Frontend.Network.Firebase;
using Frontend.Service;

namespace Frontend.Network;

public abstract class NSwagBaseClient
{
    protected const string DEFAULT_POSTER_URL = "/Images/NoPosterAvailable.webp";
    protected HttpClient _httpClient;
    protected Client _api;

    protected NSwagBaseClient(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        string backendApiUrl = configuration.GetValue<string>("BackendApiUrl");
        _httpClient =  clientFactory.CreateClient("BackendApi");
        _api = new Client(backendApiUrl, _httpClient);
    }
}

