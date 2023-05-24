using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.SocialFeed.Application.GetFollowing;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using NSubstitute;

namespace TestBackend.Social.Application.GetFollows;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    
    private readonly ISocialUserRepository _userRepository = Substitute.For<ISocialUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_userRepository, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNotFollowingOtherUsers()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, new List<string>())
            .Create();

        _userRepository.ReadSocialUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true)
            .Returns(user);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result.FollowingUserDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfIds_WhenFollowingOtherUsers()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, _fixture.Create<List<string>>())
            .Create();

        _userRepository.ReadSocialUserAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true)
            .Returns(user);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(user.Following.Count, result.FollowingUserDtos.Count);
        Assert.All(result.FollowingUserDtos, id => Assert.Contains(id, user.Following));
    }
}