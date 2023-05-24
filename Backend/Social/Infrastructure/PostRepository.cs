using Backend.Database.Transaction;
using Backend.SocialFeed.Application.GetFeedForUser;
using Backend.SocialFeed.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.SocialFeed.Infrastructure;

public class PostRepository : IPostRepository
{
    private int numberOfPostPerPage;

    public PostRepository(IConfiguration configuration)
    {
        numberOfPostPerPage = configuration.GetSection("QueryConstants").GetValue<int>("PostsPerPage");
    }

    public async Task<Post> ReadPostFromIdAsync(string id, DbTransaction tx, bool includeComments = false,
        bool includeReactions = false)
    {
        var query = tx.DataContext.Posts
            .Where(p => p.Id == id);
        if (includeComments)
        {
            query = query.Include(p => p.Comments);
        }

        if (includeReactions)
        {
            query = query.Include(p => p.Reactions);
        }

        var post = await query.FirstOrDefaultAsync();
        if (post == null)
        {
            throw new KeyNotFoundException($"Could not find post with id: {id}");
        }


        return ToDomain(post);
    }

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

    public async Task UpdateAsync(Domain.Post domainPost, DbTransaction tx)
    {
        tx.AddDomainEvents(domainPost.ReadAllDomainEvents());
        var postDao = await tx.DataContext.Posts
            .Include(p => p.Comments)
            .Include(p => p.Reactions)
            .SingleAsync(p => p.Id == domainPost.Id.ToString());

        if (domainPost.Comments != null)
        {
            if (postDao.Comments == null)
            {
                postDao.Comments = new List<CommentDAO>();
            }

            FromDomain(postDao.Comments, domainPost.Comments);
        }

        if (domainPost.Reactions != null)
        {
            if (postDao.Reactions == null)
            {
                postDao.Reactions = new List<ReactionEntryDAO>();
            }
            
            FromDomain(postDao.Reactions, domainPost.Reactions, domainPost.Id);
        }

        tx.DataContext.Posts.Update(postDao);
    }

    public async Task<List<Post>> GetFeedWithPostsFromUsersAsync(List<string> userIds, int requestedPageNumber, DbReadOnlyTransaction tx, bool includeComments = false, bool includeReactions = false)
    {
        var query = tx.DataContext.Posts
            .Include(p => p.ActivityData)
            .Where(p => userIds.Contains(p.UserId));

        if (includeComments)
        {
            query = query.Include(p => p.Comments);
        }

        if (includeReactions)
        {
            query = query.Include(p => p.Reactions);

        }
        
        var posts = await query
            .OrderByDescending(p => p.TimeOfActivity)
            .Skip(numberOfPostPerPage * (requestedPageNumber - 1))
            .Take(numberOfPostPerPage).ToListAsync();     
        
        return ToDomain(posts);
    }

    private void FromDomain(List<CommentDAO> postDaoComments, List<Comment> domainPostComments)
    {
        var commentIds = domainPostComments.Select(c => c.Id.ToString()).ToList();
        postDaoComments.RemoveAll(pc => !commentIds.Contains(pc.Id));

        foreach (var comment in domainPostComments)
        {
            var existingComment = postDaoComments.FirstOrDefault(c => c.Id == comment.Id.ToString());

            if (existingComment == null)
            {
                postDaoComments.Add(new CommentDAO
                {
                    Id = comment.Id.ToString(),
                    UserId = comment.UserId,
                    Contents = comment.Contents,
                    TimeStamp = comment.TimeStamp,
                });
            }
            else
            {
                existingComment.Contents = comment.Contents;
            }
        }
    }

    private void FromDomain(List<ReactionEntryDAO> postDaoReactions, Dictionary<string, TypeOfReaction> domainPostReactions, Guid postId)
    {
        var reactionIds = domainPostReactions.Keys.ToList();
        postDaoReactions.RemoveAll(r => !reactionIds.Contains(r.UserId));

        foreach (var kvp in domainPostReactions)
        {
            var reactionIdString = kvp.Key;
            var typeOfReaction = kvp.Value;

            var existingReaction = postDaoReactions.FirstOrDefault(r => r.UserId == reactionIdString);

            if (existingReaction == null)
            {
                postDaoReactions.Add(new ReactionEntryDAO
                {
                    PostId = postId.ToString(),
                    UserId = reactionIdString,
                    TypeOfReaction = typeOfReaction
                });
            }
            else
            {
                existingReaction.TypeOfReaction = typeOfReaction;
            }
        }
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
            Id = new Guid(dao.Id),
            UserId = dao.UserId,
            Topic = dao.Topic,
            ActivityData = ToDomain(dao.ActivityData),
            TimeOfActivity = dao.TimeOfActivity,
            Comments = ToDomain( dao.Comments),
            Reactions = ToDomain(dao.Reactions)
        };
    }

    private List<Domain.Comment>? ToDomain(List<CommentDAO>? commentDaos)
    {
        if (commentDaos == null)
        {
            return null;
        }

        var domainComments = new List<Comment>();
        foreach (var commentDao in commentDaos)
        {
            domainComments.Add(new Comment
            {
                Id = new Guid(commentDao.Id),
                UserId = commentDao.UserId,
                Contents = commentDao.Contents,
                TimeStamp = commentDao.TimeStamp,
            });
        }

        return domainComments;
    }

    private Dictionary<string, TypeOfReaction>? ToDomain(List<ReactionEntryDAO>? reactionDaos)
    {
        if (reactionDaos == null)
        {
            return null;
        }

        var domainReactions = new Dictionary<string, TypeOfReaction>();
        foreach (var reactionDao in reactionDaos)
        {
            domainReactions.Add(reactionDao.UserId, reactionDao.TypeOfReaction);
        }

        return domainReactions;
    }
    private Domain.ActivityData? ToDomain(ActivityDAO? dao)
    {
        if (dao == null)
        {
            return null;
        }

        var activity = new ActivityData
        {
            MovieId = dao.MovieId,
            NewRating = dao.NewRating,
            OldRating = dao.OldRating,
            ReviewBody = dao.ReviewBody
        };

        return activity;
    }
}