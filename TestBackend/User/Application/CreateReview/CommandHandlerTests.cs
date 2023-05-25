using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.User.Application.CreateReview;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.CreateReview;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_repository, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldAddReview_WhenUserIdValid()
    {
        // Arange
        var command = _fixture.Create<Command>();
        var user = _fixture.Create<Backend.User.Domain.User>();

        _repository.ReadUserFromIdAsync(command.userId, Arg.Any<DbTransaction>(), includeReviews: true)
            .Returns(user);
        // Act

        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _repository.Received(1).UpdateAsync(user, Arg.Any<DbTransaction>());
    }

    [Fact]
    public async Task Handle_ShouldAddReviewToRightUser_WhenUserIdValid()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Id, command.userId)
            .Create();
        
        _repository.ReadUserFromIdAsync(command.userId, Arg.Any<DbTransaction>(), includeReviews: true)
            .Returns(user);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        await _repository.Received(1)
            .ReadUserFromIdAsync(command.userId, Arg.Any<DbTransaction>(), includeReviews: true);
        await _repository.Received(1).UpdateAsync(user, Arg.Any<DbTransaction>());
    }

}