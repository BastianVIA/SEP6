using Frontend.Entities;
using Frontend.Network.SocialFeed;

namespace Frontend.Model.SocialFeed;

public class SocialFeedModel : ISocialFeedModel
{
    private ISocialFeedClient _client;

    public SocialFeedModel(ISocialFeedClient client)
    {
        _client = client;
    }
    
    public async Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null)
    {
        return await _client.GetSocialFeed(userToken, pageNumber);
    }
}