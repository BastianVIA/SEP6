using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Enum;
using Backend.User.Application.SearchUser;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.SearchUser;

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
    public async Task Handle_ReturnsResponseWithEmptyList_WhenNoUsersFound()
    {
        // Arrange
        var query = _fixture.Build<Query>()
            .With(q => q.userName, "AUserNameWhichDoesNotExistInTheSystem")
            .Create();
        var expectedList = new List<Backend.User.Domain.User>();

        _repository.SearchForUserAsync(Arg.Any<string>(), Arg.Any<UserSortingKey>(), Arg.Any<SortingDirection>(),
            Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedList);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.UserDtos);
    }

    [Fact]
    public async Task Handle_ReturnsListOfUsers_WhenNamesPartiallyMatch()
    {
        var query = _fixture.Create<Query>();
        var expectedList = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.DisplayName, $"NameWhichContains{query.userName}SearchTerm")
            .CreateMany()
            .ToList();
        
        _repository.SearchForUserAsync(Arg.Any<string>(), Arg.Any<UserSortingKey>(), Arg.Any<SortingDirection>(),
                Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedList);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        foreach (var userDto in result.UserDtos)
        {
            Assert.Contains(query.userName, userDto.DisplayName);
        }
    }


}