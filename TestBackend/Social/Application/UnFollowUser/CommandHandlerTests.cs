

using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Social.Application.UnFollowUser;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using NSubstitute;

namespace TestBackend.Social.Application.UnFollowUser;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();

    private readonly ISocialUserRepository _repository = Substitute.For<ISocialUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_repository, _transactionFactory);
    }
    
    [Fact]
    public async Task Handle_ShouldRemoveOtherUserFromFollowList_WhenValidCommand()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, new List<string>{command.UserToUnFollow})
            .Create();
        
        _repository.ReadSocialUserAsync(command.UserId, Arg.Any<DbTransaction>(), includeFollowing: true)
            .Returns(user);

        // Act

        await _handler.Handle(command, CancellationToken.None);
        // Assert
        
        await _repository.Received(1).UpdateSocialUserAsync(user, Arg.Any<DbTransaction>());
        
    }

    [Fact]
    public async Task Handle_ShouldRemoveUserFollowOnRightUser_WhenValidCommand()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var user = _fixture.Build<SocialUser>()
            .With(su => su.Id, command.UserId)
            .With(su => su.Following, new List<string>{command.UserToUnFollow})
            .Create();

        _repository.ReadSocialUserAsync(command.UserId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true)
            .Returns(user);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _repository.Received(1)
            .ReadSocialUserAsync(command.UserId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true);

    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_OnValidCommand()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var user = _fixture.Build<SocialUser>()
            .With(u => u.Following, new List<string>{command.UserToUnFollow})
            .Create();
        
        _repository.ReadSocialUserAsync(command.UserId, Arg.Any<DbReadOnlyTransaction>(), includeFollowing: true)
            .Returns(user);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        await _repository.UpdateSocialUserAsync(user, Arg.Any<DbTransaction>());
    }
    
}