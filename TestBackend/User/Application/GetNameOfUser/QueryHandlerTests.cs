using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.User.Application.GetNameOfUser;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.GetNameOfUser;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnNameOfUser_WhenUserExists()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Create<Backend.User.Domain.User>();
        
        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(user);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(user.DisplayName, result);

    }

    [Fact]
    public async Task Handle_ShouldReturnNameOfRightUser_WhenUserExists()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Id, query.Id)
            .Create();
        
        _repository.ReadUserFromIdAsync(query.Id, Arg.Any<DbReadOnlyTransaction>())
            .Returns(user);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Equal(user.DisplayName, result);
    }
}