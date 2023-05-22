using Frontend.Entities;
using Frontend.Network.Top100;

namespace Frontend.Model.Top100;

public class Top100Model : ITop100Model
{
    private ITop100Client _client;

    public Top100Model()
    {
        _client = new Top100Client();
    }

    public async Task<List<Movie>> GetTop100List()
    {
        return await _client.GetTop100();
    }
}