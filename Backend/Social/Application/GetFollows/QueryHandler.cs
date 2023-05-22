using Backend.Database.TransactionManager;
using Backend.SocialFeed.Application.GetFeedForUser;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.SocialFeed.Application.GetFollowing;

public record Query(string userId) : IRequest<GetFollowingResponse>;

public record GetFollowingResponse(List<string>? FollowingUserDtos);

public class GetFollowingUserDto
{
    public string Id { get; set; }
}

public class QueryHandler : IRequestHandler<Query, GetFollowingResponse>
{
    private readonly ISocialUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler(ISocialUserRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<GetFollowingResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var user = await _repository.ReadSocialUserAsync(request.userId, transaction, includeFollowing: true);
        return toDto(user);
    }

    private GetFollowingResponse toDto(Domain.SocialUser user)
    {
        return new GetFollowingResponse(user.Following);
    }
}