using Backend.Database.Transaction;
using Backend.SocialFeed.Domain;

namespace Backend.SocialFeed.Infrastructure;

public interface IPostRepository
{
    Task CreatePostAsync(Domain.Post post, DbTransaction tx);
    Task<List<Post>> GetFeedWithPostsFromUsers(List<string> userIds, int requestedPageNumber, DbReadOnlyTransaction tx);
}