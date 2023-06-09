﻿using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Social.Application.FollowUser;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using NSubstitute;

namespace TestBackend.Social.Application.FollowUser;

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
    public async Task Handle_ShouldAddOtherUserToFollowList_WhenValidCommand()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var user = _fixture.Create<SocialUser>();

        _repository.ReadSocialUserAsync(command.userId, Arg.Any<DbTransaction>(), includeFollowing: true)
            .Returns(user);
        // Act

        await _handler.Handle(command, CancellationToken.None);
        // Assert
      
        await _repository.Received(1).UpdateSocialUserAsync(user, Arg.Any<DbTransaction>());
    }

    [Fact]
    public async Task Handle_ShouldAddOtherUserToFollowListOnRightSocialUser_WhenValidCommand()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var user = _fixture.Build<SocialUser>()
            .With(su => su.Id, command.userId)
            .Create();

        _repository.ReadSocialUserAsync(command.userId, Arg.Any<DbTransaction>(), includeFollowing: true)
            .Returns(user);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _repository.Received(1)
            .ReadSocialUserAsync(command.userId, Arg.Any<DbTransaction>(), includeFollowing: true);
        await _repository.Received(1).UpdateSocialUserAsync(user, Arg.Any<DbTransaction>());

    }
}