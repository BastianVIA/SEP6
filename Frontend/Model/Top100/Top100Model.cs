using Frontend.Entities;
using Frontend.Network.Top100;

namespace Frontend.Model.Top100;

public class Top100Model : ITop100Model
{
    private ITopClient _client;

    public Top100Model()
    {
        _client = new TopClient();
    }

    public async Task<List<Movie>> GetTopList(int pageNumber)
    {
        return await _client.GetTop100(pageNumber);
    }
}