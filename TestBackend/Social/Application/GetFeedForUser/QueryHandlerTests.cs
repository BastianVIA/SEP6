using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.SocialFeed.Application.GetFeedForUser;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using MediatR;
using NSubstitute;

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
    public async Task Handle_ShouldReturnEmptyList_WhenNotFollowingOtherUsers()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, new List<string>())
            .Create();
        
        var emptyList = new List<Post>();

        _userRepository.ReadSocialUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true)
            .Returns(user);
        _postRepository.GetFeedWithPostsFromUsersAsync(user.Following, Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>(),
            includeComments: true, includeReactions: true)
            .Returns(emptyList);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Empty(result.FeedPostDtos);
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

        _userRepository.ReadSocialUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true)
            .Returns(user);
        _postRepository.GetFeedWithPostsFromUsersAsync(user.Following, Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>(),
            includeComments: true, includeReactions: true)
            .Returns(expectedReturn);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(expectedReturn.Count, result.FeedPostDtos.Count);
        Assert.All(result.FeedPostDtos, dto => Assert.Contains(dto.UserId, user.Following));
    }
}