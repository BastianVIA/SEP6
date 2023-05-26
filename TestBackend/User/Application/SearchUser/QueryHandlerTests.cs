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
    private readonly IUserImageRepository _imageRepository = Substitute.For<IUserImageRepository>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _transactionFactory, _imageRepository);
    }

    [Fact]
    public async Task Handle_ReturnsResponseWithEmptyList_WhenNoUsersFound()
    {
        // Arrange
        var query = _fixture.Build<Query>()
            .With(q => q.userName, "AUserNameWhichDoesNotExistInTheSystem")
            .Create();
        var returnedPeople = new List<Backend.User.Domain.User>();
        var expectedReturn = returnedPeople;
        byte[]? expectedImage = null;

        _repository.SearchForUserAsync(Arg.Any<string>(), Arg.Any<UserSortingKey>(), Arg.Any<SortingDirection>(),
            Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedReturn);
        _imageRepository.ReadImageForUserAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedImage);
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
        var returnedPeople = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.DisplayName, $"NameWhichContains{query.userName}SearchTerm")
            .CreateMany()
            .ToList();
        var expectedReturn = returnedPeople;
        byte[]? expectedImage = _fixture.Create<byte[]>();

        _repository.SearchForUserAsync(Arg.Any<string>(), Arg.Any<UserSortingKey>(), Arg.Any<SortingDirection>(),
                Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedReturn);
        _imageRepository.ReadImageForUserAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedImage);
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        foreach (var userDto in result.UserDtos)
        {
            Assert.Contains(query.userName, userDto.DisplayName);
            Assert.Equal(expectedImage, userDto.Image);
        }
    }


}