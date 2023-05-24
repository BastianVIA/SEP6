using Backend.Database.TransactionManager;
using Backend.SocialFeed.Application.GetFeedForUser;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.Social.Application.GetPostsForUsers;

public record Query(List<string> Users, string RequestedUserId, int PageNumber) : IRequest<GetPostsForUsersResponse>;

public record GetPostsForUsersResponse(List<FeedPostDto> FeedPostDtos);

public class FeedPostDto
{
    public Guid Id;
    public string UserId;
    public string UserDisplayname;
    public Activity Topic;
    public ActivityDataDto? ActivityDataDto;
    public int numberOfReactions;
    public List<FeedCommentDto>? Comments;
    public DateTime TimeOfActivity;
    public TypeOfReaction? UsersReaction;
}

public class ActivityDataDto
{
    public string? MovieId { get; set; }
    public string? MovieTitle { get; set; }
    public int? NewRating { get; set; }
    public int? OldRating { get; set; }
    
    public string? ReviewBody { get; set; }
}

public class FeedCommentDto
{
    public Guid Id { get; set; }
    public string DisplayNameOfUser { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; }
}

    
public class QueryHandler : IRequestHandler<Query,GetPostsForUsersResponse >
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _postRepository;
    private readonly IMediator _mediator;


    public QueryHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository postRepository,  IMediator mediator)
    {
        _transactionFactory = transactionFactory;
        _postRepository = postRepository;
        _mediator = mediator;
    }

    public async Task<GetPostsForUsersResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var feedForUser = await _postRepository.GetFeedWithPostsFromUsersAsync(request.Users, request.PageNumber, transaction, includeComments:true, includeReactions:true);

        return await toDto(feedForUser, request.RequestedUserId);
    }
    
     private async Task<GetPostsForUsersResponse> toDto(IList<Post> listOfPosts, string requestingUser)
    {
        List<FeedPostDto> dtoPosts = new List<FeedPostDto>();
        foreach (var post in listOfPosts)
        {
            dtoPosts.Add(await toDto(post, requestingUser));
        }

        return new GetPostsForUsersResponse(dtoPosts);
    }

    private async Task<FeedPostDto> toDto(Post post, string requestingUser)
    {
        var noOfReactions = 0;
        if (post.Reactions != null)
        {
            noOfReactions = post.Reactions.Count;
        }
        var displayNameOfUser = _mediator.Send(new User.Application.GetNameOfUser.Query(post.UserId));

        return new FeedPostDto
        {
            Id = post.Id,
            UserId = post.UserId,
            UserDisplayname = await displayNameOfUser,
            Topic = post.Topic,
            ActivityDataDto = await toDto(post.ActivityData),
            TimeOfActivity = post.TimeOfActivity,
            Comments = await toDto(post.Comments),
            numberOfReactions = noOfReactions,
            UsersReaction = post.GetUsersReaction(requestingUser)
        };
    }

    private async Task<ActivityDataDto?> toDto(ActivityData? activityData)
    {
        if (activityData == null)
        {
            return null;
        }

        string? movieTitle = null;
        if (activityData.MovieId != null)
        {
            movieTitle = await _mediator.Send(new Movie.Application.GetTitle.Query(activityData.MovieId));
        }
        return new ActivityDataDto
        {
            MovieId = activityData.MovieId,
            MovieTitle = movieTitle,
            NewRating = activityData.NewRating,
            OldRating = activityData.OldRating,
            ReviewBody = activityData.ReviewBody
        };
    }

    private async Task<List<FeedCommentDto>> toDto(List<Comment>? comments)
    {
        var dtoComments = new List<FeedCommentDto>();
        if (comments == null)
        {
            return dtoComments;
        }

        foreach (var comment in comments)
        {
            var displayNameOfUser = _mediator.Send(new User.Application.GetNameOfUser.Query(comment.UserId));
            dtoComments.Add(new FeedCommentDto
            {
                Id = comment.Id,
                DisplayNameOfUser =await displayNameOfUser,
                UserId = comment.UserId,
                Content = comment.Contents,
                TimeStamp = comment.TimeStamp
            });
        }

        return dtoComments;
    }
}