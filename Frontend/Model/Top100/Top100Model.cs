using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.Top100;

namespace Frontend.Model.Top100;

public class Top100Model :ITop100Model
{
    private ITop100Client _client;

    public Top100Model(ITop100Client client)
    {
        _client = client;
    }

    public async Task<List<Movie>> GetTopList(int pageNumber)
    {
        return await _client.GetTop100(pageNumber);
    }
}