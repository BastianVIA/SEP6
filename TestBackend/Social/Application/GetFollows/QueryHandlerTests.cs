using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Social.Application.GetFollows;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using MediatR;
using NSubstitute;

namespace TestBackend.Social.Application.GetFollows;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    
    private readonly ISocialUserRepository _userRepository = Substitute.For<ISocialUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_userRepository, _transactionFactory, _mediator);
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
    
}