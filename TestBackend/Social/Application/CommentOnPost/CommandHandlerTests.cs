using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Social.Application.CommentOnPost;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReceivedExtensions;

namespace TestBackend.Social.Application.CommentOnPost;

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
    public async Task Handle_ShouldAddUpdateRepositoryOnce_WhenNoErrors()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var post = _fixture.Create<Post>();

        _repository.ReadPostFromIdAsync(command.PostId, Arg.Any<DbTransaction>(), includeComments: true)
            .Returns(post);
        // Act

        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _repository.Received(1).UpdateAsync(post, Arg.Any<DbTransaction>());
    }

    [Fact]
    public async Task Handle_AddsCommentToRightPost_WhenNoErrors()
    {
        // Arrange
        var command = _fixture.Create<Command>();
        var post = _fixture.Build<Post>()
            .With(p => p.Id)
            .Create();

        _repository.ReadPostFromIdAsync(command.PostId, Arg.Any<DbTransaction>(), includeComments: true)
            .Returns(post);
        
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        await _repository.Received(1)
            .ReadPostFromIdAsync(command.PostId, Arg.Any<DbTransaction>(), includeComments: true);
    }


}