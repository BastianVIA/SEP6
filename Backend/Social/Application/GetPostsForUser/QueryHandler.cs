using Backend.Database.TransactionManager;
using Backend.Social.Application.GetPostsForUsers;
using Backend.SocialFeed.Application.GetFeedForUser;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.Social.Application.GetPostsForUser;

public record Query(string userId, int pageNumber) : IRequest<GetPostsForUserResponse>;

public record GetPostsForUserResponse(GetPostsForUsersResponse GetPostsForUsersResponse);


public class QueryHandler : IRequestHandler<Query, GetPostsForUserResponse>
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

    public async Task<GetPostsForUserResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var feedForUser = await _mediator.Send(new GetPostsForUsers.Query(new List<string>(){request.userId}, request.userId, request.pageNumber));
        return new GetPostsForUserResponse(feedForUser);
    }
}