using Backend.Database.Transaction;
using Backend.SocialFeed.Domain;
using FirebaseAdmin.Auth;

namespace Backend.SocialFeed.Infrastructure;

public interface ISocialUserRepository
{
    Task<SocialUser>ReadSocialUserAsync(string id, DbReadOnlyTransaction tx, bool includeFollowing = false);
    void CreateSocialUser(string userId , DbTransaction tx);
    Task UpdateSocialUser(Domain.SocialUser socialUser, DbTransaction tx);
}