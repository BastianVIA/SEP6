using Frontend.Entities;

namespace Frontend.Network.Top100;

public interface ITop100Client
{
    public Task<List<Movie>> GetTop100();
}