using Frontend.Entities;
using Frontend.Network.SocialFeed;

namespace Frontend.Model.SocialFeed;

public class SocialFeedModel : ISocialFeedModel
{
    private ISocialFeedClient _client;

    public SocialFeedModel()
    {
        _client = new SocialFeedClient();
    }
    
    public async Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null)
    {
        return await _client.GetSocialFeed(userToken, pageNumber);
    }

    public async Task ReactToSocialFeed(string userToken, string postId)
    {
        await _client.ReactToSocialFeed(userToken, postId);
    }

    public async Task CommentOnPost(string userToken, string postId, string comment)
    {
        await _client.CommentOnPost(userToken, postId, comment);
    }

    public async Task<List<UserFeed>> GetOwnSocialFeed(string userToken, int? pageNumber = null)
    {
        return await _client.GetOwnSocialFeed(userToken, pageNumber);
    }
}