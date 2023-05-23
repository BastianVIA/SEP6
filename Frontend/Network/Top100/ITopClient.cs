using Frontend.Entities;

namespace Frontend.Network.Top100;

public interface ITopClient
{
    public Task<List<Movie>> GetTop100(int pageNumber);
}