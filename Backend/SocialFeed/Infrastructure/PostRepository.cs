using Backend.Database.Transaction;
using Backend.SocialFeed.Application.GetFeedForUser;
using Backend.SocialFeed.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.SocialFeed.Infrastructure;

public class PostRepository : IPostRepository
{
    private const int numberOfPostPerPage = 10;

    public async Task CreatePostAsync(Post post, DbTransaction tx)
    {
        tx.AddDomainEvents(post.ReadAllDomainEvents());
        ActivityDAO? activityData = null;
        if (post.ActivityData != null)
        {
            activityData = new ActivityDAO
            {
                Id = Guid.NewGuid().ToString(),
                PostId = post.Id.ToString(),
                MovieId = post.ActivityData.MovieId,
                NewRating = post.ActivityData.NewRating,
                OldRating = post.ActivityData.OldRating,
                ReviewBody = post.ActivityData.ReviewBody
            };
        }

        await tx.DataContext.Posts.AddAsync(new PostDAO
        {
            Id = post.Id.ToString(),
            UserId = post.UserId,
            Topic = post.Topic,
            TimeOfActivity = post.TimeOfActivity,
            ActivityData = activityData

        });
    }

  
    public async Task<List<Post>> GetFeedWithPostsFromUsers(List<string> userIds, int requestedPageNumber, DbReadOnlyTransaction tx)
    {
        var posts = await tx.DataContext.Posts
            .Where(p => userIds.Contains(p.UserId))
            .Include(p => p.ActivityData)
            .OrderBy(p => p.TimeOfActivity)
            .Skip(numberOfPostPerPage * (requestedPageNumber - 1))
            .Take(numberOfPostPerPage).ToListAsync();
        return ToDomain(posts);
    }

    private List<Domain.Post> ToDomain(List<PostDAO>? daos)
    {
        if (daos == null || !daos.Any())
        {
            return new List<Post>();
        }
        var domainList = new List<Domain.Post>();
        foreach (var postDao in daos)
        {
            domainList.Add(ToDomain(postDao));
        }

        return domainList;
    }

    private Domain.Post ToDomain(PostDAO dao)
    {

        return new Post
        {
            Id =  new Guid(dao.Id),
            UserId = dao.UserId,
            Topic = dao.Topic,
            ActivityData = ToDomain(dao.ActivityData),
            TimeOfActivity = dao.TimeOfActivity
        };
    }
    
    private Domain.ActivityData? ToDomain(ActivityDAO? dao)
    {
        if (dao == null)
        {
            return null;
        }
        var activity = new ActivityData();
        if (dao.MovieId != null)
        {
            activity.MovieId = dao.MovieId;
        }

        if (dao.NewRating != null)
        {
            activity.NewRating = dao.NewRating;
        }

        if (dao.OldRating != null)
        {
            activity.OldRating = dao.OldRating;
        }

        if (dao.ReviewBody != null)
        {
            activity.ReviewBody = dao.ReviewBody;
        }

        return activity;
    }
}