using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Social.Application.GetPostsForUser;
using Backend.Social.Application.GetPostsForUsers;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using MediatR;
using NSubstitute;
using Query = Backend.Social.Application.GetFeedForUser.Query;
using QueryHandler = Backend.Social.Application.GetFeedForUser.QueryHandler;

namespace TestBackend.Social.Application.GetFeedForUser;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly IPostRepository _postRepository = Substitute.For<IPostRepository>();
    private readonly ISocialUserRepository _userRepository = Substitute.For<ISocialUserRepository>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_transactionFactory, _postRepository, _userRepository, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNotFollowingOtherUsers()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, new List<string>())
            .Create();
        
        var emptyList = new List<Post>();

        _userRepository.ReadSocialUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>())
            .Returns(user);
        _postRepository.GetFeedWithPostsFromUsersAsync(user.Following, Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>(),
            Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(emptyList);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Null(result.GetPostsForUsersResponse);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfPostsFromFollowedUser_WhenFollowingUsers()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, _fixture.Create<List<string>>())
            .Create();

        var expectedReturn = user.Following.Select(
            id => _fixture.Build<Post>()
                .With(p => p.UserId, id)
                .Create())
            .ToList();


        var expectedPosts = _fixture.Build<GetPostsForUsersResponse>()
            .With(r => r.FeedPostDtos, user.Following.Select(
                id => _fixture.Build<FeedPostDto>()
                    .With(f =>f.UserId, id)
                    .Create())
                .ToList()
                )
            .Create();

        _userRepository.ReadSocialUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>())
            .Returns(user);
        _postRepository.GetFeedWithPostsFromUsersAsync(user.Following, Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>(),
                Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(expectedReturn);
        _mediator.Send(Arg.Any<Backend.Social.Application.GetPostsForUsers.Query>())
            .Returns(expectedPosts);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(expectedReturn.Count, result.GetPostsForUsersResponse.FeedPostDtos.Count);
        Assert.All(result.GetPostsForUsersResponse.FeedPostDtos, dto => Assert.Contains(dto.UserId, user.Following));
    }
}