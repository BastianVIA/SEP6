using AutoFixture;
using Backend.Service;
using Backend.User.Application.SetUserImage;
using NSubstitute;

namespace TestBackend.User.Application.SetUserImage;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserImageService _userImageService = Substitute.For<IUserImageService>();

    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_userImageService);
    }

    [Fact]
    public async Task Handle_CallsServiceToUpdate_WhenCommandProvided()
    {
        var command = _fixture.Create<Command>();

        await _handler.Handle(command, CancellationToken.None);
        await _userImageService.Received(1).UploadImageAsync(command.userId, command.data);
    }
}