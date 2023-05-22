using Frontend.Entities;

namespace Frontend.Model.SocialFeed;

public interface ISocialFeedModel
{
    public Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null);
}