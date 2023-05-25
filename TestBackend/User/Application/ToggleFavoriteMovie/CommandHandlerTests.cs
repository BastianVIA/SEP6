using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.User.Application.ToggleFavoriteMovie;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.ToggleFavoriteMovie;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserRepository _repository = Substitute.For<IUserRepository>();

    private readonly IDatabaseTransactionFactory _databaseTransactionFactory =
        Substitute.For<IDatabaseTransactionFactory>();

    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_repository, _databaseTransactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReadUserWithFavouritesIncluded()
    {
        var command = _fixture.Create<Command>();
        var user = _fixture.Create<Backend.User.Domain.User>();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbTransaction>(), Arg.Any<bool>(), Arg.Any<bool>(),
            Arg.Any<bool>())
            .Returns(user);

        await _handler.Handle(command, CancellationToken.None);

        await _repository.Received(1)
            .ReadUserFromIdAsync(command.userId, Arg.Any<DbTransaction>(), includeFavoriteMovies: true);
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenValidCommand()
    {
        var command = _fixture.Create<Command>();
        var user = _fixture.Create<Backend.User.Domain.User>();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbTransaction>(), Arg.Any<bool>(), Arg.Any<bool>(),
                Arg.Any<bool>())
            .Returns(user);

        await _handler.Handle(command, CancellationToken.None);

        await _repository.Received(1)
            .UpdateAsync(user, Arg.Any<DbTransaction>());
    }
}