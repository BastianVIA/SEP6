using Backend.Database.TransactionManager;
using Backend.Social.Application.GetPostsForUsers;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.Social.Application.GetFeedForUser;

public record Query(string userId, int pageNumber) : IRequest<GetFeedForUserResponse>;

public record GetFeedForUserResponse(GetPostsForUsersResponse? GetPostsForUsersResponse = null);

public class QueryHandler : IRequestHandler<Query, GetFeedForUserResponse>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _postRepository;
    private readonly ISocialUserRepository _userRepository;
    private readonly IMediator _mediator;
    
    public QueryHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository postRepository, ISocialUserRepository userRepository, IMediator mediator)
    {
        _transactionFactory = transactionFactory;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _mediator = mediator;
    }

    // public async Task<GetFeedForUserResponse> Handle(Query request, CancellationToken cancellationToken)
    // {
    //     var transaction = _transactionFactory.BeginReadOnlyTransaction();
    //     var user = await _userRepository.ReadSocialUserAsync(request.userId, transaction, includeFollowing: true);
    //     if (user.Following == null)
    //     {
    //         return new GetFeedForUserResponse();
    //     }
    //
    //     var feedForUser = await _mediator.Send(new GetPostsForUsers.Query(user.Following, user.Id, request.pageNumber));
    //     return new GetFeedForUserResponse(feedForUser);
    // }
    //
    
    public async Task<GetFeedForUserResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.userId))
        {
            throw new ArgumentException("UserId must not be null or empty.", nameof(request.userId));
        }

        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var user = await _userRepository.ReadSocialUserAsync(request.userId, transaction, includeFollowing: true);

        if (user == null)
        {
            return new GetFeedForUserResponse(); 
        }

        if (user.Following == null)
        {
            return new GetFeedForUserResponse();
        }

        var feedForUser = await _mediator.Send(new GetPostsForUsers.Query(user.Following, user.Id, request.pageNumber));
        return new GetFeedForUserResponse(feedForUser);
    }

}