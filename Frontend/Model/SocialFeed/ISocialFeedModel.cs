using Frontend.Entities;

namespace Frontend.Model.SocialFeed;

public interface ISocialFeedModel
{
    public Task<List<UserFeed>> GetSocialFeed(string userToken, int? pageNumber = null);
    public Task ReactToSocialFeed(string userToken, string postId);
    
    public Task CommentOnPost(string userToken, string postId, string comment);
    public Task<List<UserFeed>> GetOwnSocialFeed(string userToken, int? pageNumber = null);
}