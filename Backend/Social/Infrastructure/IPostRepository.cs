using Backend.Database.Transaction;
using Backend.SocialFeed.Domain;

namespace Backend.SocialFeed.Infrastructure;

public interface IPostRepository
{
    Task<Post> ReadPostFromId(string id, DbTransaction tx, bool includeComments = false,
        bool includeReactions = false);
    Task CreatePostAsync(Domain.Post post, DbTransaction tx);
    Task<List<Post>> GetFeedWithPostsFromUsers(List<string> userIds, int requestedPageNumber, DbReadOnlyTransaction tx, bool includeComments = false, bool includeReactions = false);
    Task UpdateAsync(Domain.Post domainPost, DbTransaction tx);

}