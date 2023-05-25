using Backend.Database.Transaction;
using Backend.Social.Domain;

namespace Backend.Social.Infrastructure;

public interface IPostRepository
{
    Task<Post> ReadPostFromIdAsync(string id, DbTransaction tx, bool includeComments = false,
        bool includeReactions = false);
    Task CreatePostAsync(Post post, DbTransaction tx);
    Task<List<Post>> GetFeedWithPostsFromUsersAsync(List<string> userIds, int requestedPageNumber, DbReadOnlyTransaction tx, bool includeComments = false, bool includeReactions = false);
    Task UpdateAsync(Post domainPost, DbTransaction tx);

}