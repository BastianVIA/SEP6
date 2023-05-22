using Frontend.Entities;
using Frontend.Network;
using Frontend.Network.Recommendations;

namespace Frontend.Model.Recommendations;

public class RecommendationsModel : NSwagBaseClient, IRecommendationsModel
{
    private IRecommendationsClient _client;
    
    public RecommendationsModel(IRecommendationsClient client,IConfiguration configuration,IHttpClientFactory clientFactory):base(clientFactory,configuration)
    {
        _client = client;
    }
    
    public async Task<List<Movie>> GetRecommendations()
    {
        return await _client.GetRecommendations();
    }
}