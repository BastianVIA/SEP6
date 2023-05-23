using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.Recommendations;

namespace Frontend.Model.Recommendations;

public class RecommendationsModel : NSwagBaseClient, IRecommendationsModel
{
    private IRecommendationsClient _client;
    
    public RecommendationsModel()
    {
        _client = new RecommendationsClient();
    }
    
    public async Task<List<Movie>> GetRecommendations()
    {
        return await _client.GetRecommendations();
    }
}