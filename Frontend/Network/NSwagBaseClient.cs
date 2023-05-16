using Frontend.Network.Firebase;
using Frontend.Service;

namespace Frontend.Network;

public abstract class NSwagBaseClient
{
    protected const string BASEURI = "http://localhost:5276";
    protected const string DEFAULT_POSTER_URL = "/Images/NoPosterAvailable.webp"; 
    protected HttpClient _httpClient;
    protected Client _api;

    protected NSwagBaseClient()
    {
        _httpClient = new HttpClient();
        _api = new Client(BASEURI, _httpClient);
    }
}