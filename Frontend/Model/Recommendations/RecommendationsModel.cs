using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.Recommendations;

namespace Frontend.Model.Recommendations;

public class RecommendationsModel : IRecommendationsModel
{
    private IRecommendationsClient _client;
    
    public RecommendationsModel(IRecommendationsClient client)
    {
        _client = client;
    }
    
    public async Task<List<Movie>> GetRecommendations()
    {
        return await _client.GetRecommendations();
    }
}