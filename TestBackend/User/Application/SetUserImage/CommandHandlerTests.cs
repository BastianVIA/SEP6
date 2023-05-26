using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Service;
using Backend.User.Application.SetUserImage;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.SetUserImage;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly IUserImageRepository _userImageService = Substitute.For<IUserImageRepository>();

    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_userImageService, _transactionFactory);
    }

    [Fact]
    public async Task Handle_CallsServiceToUpdate_WhenCommandProvided()
    {
        var command = _fixture.Create<Command>();

        await _handler.Handle(command, CancellationToken.None);
        await _userImageService.Received(1).UploadImageAsync(command.userId, command.data, _fixture.Create<DbTransaction>());
    }
}