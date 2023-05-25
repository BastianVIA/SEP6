using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.User.Application.CreateUser;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.CreateUser;

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
    public async Task Handle_ShouldUpdateRepository_WhenReceivedCommand()
    {
        // Arrange
        var command = _fixture.Create<Command>();

        // Act & Assert
        await _handler.Handle(command, CancellationToken.None);
        await _repository.Received(1).CreateUserAsync(Arg.Any<Backend.User.Domain.User>(), Arg.Any<DbTransaction>());
    }
}