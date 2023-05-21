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
    public DateTime TimeOfActivity { get; set; }
}

public class ActivityDataDto
{
    public string? MovieId { get; set; }
    public int? NewRating { get; set; }
    public int? OldRating { get; set; }
    
    public string? ReviewBody { get; set; }
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
        var feedForUser = await _postRepository.GetFeedWithPostsFromUsers(user.Following, request.pageNumber, transaction);
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
        return new FeedPostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            Topic = post.Topic,
            ActivityDataDto = toDto(post.ActivityData),
            TimeOfActivity = post.TimeOfActivity
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
            ReviewBody = activityData.ReviewBody
        };
    }
}