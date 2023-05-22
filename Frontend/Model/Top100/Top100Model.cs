using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.Top100;

namespace Frontend.Model.Top100;

public class Top100Model :NSwagBaseClient, ITop100Model
{
    private ITop100Client _client;

    public Top100Model(IConfiguration configuration,IHttpClientFactory clientFactory):base(clientFactory,configuration)
    {
        _client = new Top100Client(clientFactory,configuration);
    }

    public async Task<List<Movie>> GetTop100List()
    {
        return await _client.GetTop100();
    }
}