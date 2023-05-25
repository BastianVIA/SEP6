using Backend.Database.Transaction;
using Backend.Social.Domain;

namespace Backend.Social.Infrastructure;

public interface ISocialUserRepository
{
    Task<SocialUser>ReadSocialUserAsync(string id, DbReadOnlyTransaction tx, bool includeFollowing = false);
    void CreateSocialUserAsync(SocialUser user , DbTransaction tx);
    Task UpdateSocialUserAsync(SocialUser socialUser, DbTransaction tx);
}