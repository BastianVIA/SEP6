using Backend.Database.TransactionManager;
using Backend.Social.Infrastructure;
using MediatR;

namespace Backend.Social.Application.GetFollows;

public record Query(string userId) : IRequest<GetFollowingResponse>;

public record GetFollowingResponse(List<GetFollowingUserDto> FollowingUserDtos);

public class GetFollowingUserDto
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
}

public class QueryHandler : IRequestHandler<Query, GetFollowingResponse>
{
    private readonly ISocialUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IMediator _mediator;

    public QueryHandler(ISocialUserRepository repository, IDatabaseTransactionFactory transactionFactory, IMediator mediator)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
        _mediator = mediator;
    }

    public async Task<GetFollowingResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var user = await _repository.ReadSocialUserAsync(request.userId, transaction, includeFollowing: true);
        return await toDto(user);
    }

    private async Task<GetFollowingResponse> toDto(Domain.SocialUser user)
    {
        List<GetFollowingUserDto> followingUserDtos = new List<GetFollowingUserDto>();
        foreach (var id in user.Following)
        {
            var displayNameOfUser = _mediator.Send(new User.Application.GetNameOfUser.Query(id));
            followingUserDtos.Add(new GetFollowingUserDto
            {
                Id = id,
                DisplayName = await displayNameOfUser
            });
        }
        return new GetFollowingResponse(followingUserDtos);
    }
}