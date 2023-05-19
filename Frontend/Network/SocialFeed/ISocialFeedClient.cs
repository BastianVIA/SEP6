using Frontend.Entities;

namespace Frontend.Network.SocialFeed;

public interface ISocialFeedClient
{
    public Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null);
}