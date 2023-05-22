using Backend.Database.TransactionManager;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.SocialFeed.Application.GetFeedForUser;

public record Query(string userId, int pageNumber) : IRequest<GetFeedForUserResponse>;

public record GetFeedForUserResponse(List<FeedPostDto> FeedPostDtos);

public class FeedPostDto
{
    public Guid Id;
    public string UserId;
    public Activity Topic;
    public ActivityDataDto? ActivityDataDto;
    public int numberOfReactions;
    public List<FeedCommentDto>? Comments;
    public DateTime TimeOfActivity { get; set; }
}

public class ActivityDataDto
{
    public string? MovieId { get; set; }
    public int? NewRating { get; set; }
    public int? OldRating { get; set; }
}

public class FeedCommentDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
}

public class QueryHandler : IRequestHandler<Query, GetFeedForUserResponse>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _postRepository;
    private readonly ISocialUserRepository _userRepository;
    
    public QueryHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository postRepository, ISocialUserRepository userRepository)
    {
        _transactionFactory = transactionFactory;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<GetFeedForUserResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var user = await _userRepository.ReadSocialUserAsync(request.userId, transaction, includeFollowing: true);
        if (user.Following == null)
        {
            return new GetFeedForUserResponse(new List<FeedPostDto>());
        }
        var feedForUser = await _postRepository.GetFeedWithPostsFromUsers(user.Following, request.pageNumber, transaction, includeComments:true, includeReactions:true);
        return toDto(feedForUser);
    }

    private GetFeedForUserResponse toDto(IList<Post> listOfPosts)
    {
        List<FeedPostDto> dtoPosts = new List<FeedPostDto>();
        foreach (var post in listOfPosts)
        {
            dtoPosts.Add(toDto(post));
        }

        return new GetFeedForUserResponse(dtoPosts);
    }

    private FeedPostDto toDto(Post post)
    {
        var noOfReactions = 0;
        if (post.Reactions != null)
        {
            noOfReactions = post.Reactions.Count;
        }
        return new FeedPostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Topic = post.Topic,
            ActivityDataDto = toDto(post.ActivityData),
            TimeOfActivity = post.TimeOfActivity,
            Comments = toDto(post.Comments),
            numberOfReactions = noOfReactions
        };
    }

    private ActivityDataDto? toDto(ActivityData? activityData)
    {
        if (activityData == null)
        {
            return null;
        }
        return new ActivityDataDto
        {
            MovieId = activityData.MovieId,
            NewRating = activityData.NewRating,
            OldRating = activityData.OldRating,
        };
    }

    private List<FeedCommentDto> toDto(List<Comment>? comments)
    {
        var dtoComments = new List<FeedCommentDto>();
        if (comments == null)
        {
            return dtoComments;
        }

        foreach (var comment in comments)
        {
            dtoComments.Add(new FeedCommentDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                Content = comment.Contents
            });
        }

        return dtoComments;
    }
}