
using Backend.SocialFeed.Domain;
using Frontend.Entities;

namespace Frontend.Network.SocialFeed;

public interface ISocialFeedClient
{
    public Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null);
    public Task ReactToSocialFeed(string userToken, string postId);
    public Task CommentOnPost(string userToken, string postId, string comment);
    public Task<List<UserFeed>> GetOwnSocialFeed(string userId, int? pageNumber = null);
}