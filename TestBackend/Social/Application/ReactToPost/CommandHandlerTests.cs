using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Social.Application.ReactToPost;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using NSubstitute;

namespace TestBackend.Social.Application.ReactToPost;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();
    private readonly IPostRepository _repository = Substitute.For<IPostRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_repository, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePost_WhenCommandValid()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var post = _fixture.Create<Post>();
        
        _repository.ReadPostFromIdAsync(command.PostId, Arg.Any<DbTransaction>(), includeReactions: true)
            .Returns(post);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        await _repository.Received(1).UpdateAsync(post, Arg.Any<DbTransaction>());
    }

    [Fact]
    public async Task Handle_ShouldAddReactionToCorrectPost_WhenCommandValid()
    {
        // Arrange
        var command = _fixture.Build<Command>()
            .With(c => c.PostId, new Guid().ToString)
            .Create();

        var post = Substitute.For<Post>();

        _repository.ReadPostFromIdAsync(command.PostId, Arg.Any<DbTransaction>(), includeReactions: true)
            .Returns(post);
        // Act

        await _handler.Handle(command, CancellationToken.None);
        // Assert
        post.Received(1).PutReaction(command.UserId, command.Reaction);
    }

    [Fact]
    public async Task Handle_ShouldUpdateRepository_WhenCommandValid()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var post = _fixture.Create<Post>();
        
        _repository.ReadPostFromIdAsync(command.PostId, Arg.Any<DbTransaction>(), includeReactions: true)
            .Returns(post);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        await _repository.Received(1)
            .UpdateAsync(post, Arg.Any<DbTransaction>());
    }
}