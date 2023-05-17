using Frontend.Entities;

namespace Frontend.Network.Recommendations;

public interface IRecommendationsClient
{
    public Task<List<Movie>> GetRecommendations();
}