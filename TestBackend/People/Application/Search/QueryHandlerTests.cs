using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.People.Application.Search;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using NSubstitute;

namespace TestBackend.People.Application.Search;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IPeopleRepository _repository = Substitute.For<IPeopleRepository>();
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory =
        Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _databaseTransactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoMatchFound()
    {
        // Arrange
        var query = _fixture.Build<Query>()
            .With(q => q.name, "ANameWhichNoPersonHasBeenXÆA-12")
            .Create();
        var expectedList = _fixture.Create<(List<Person>, int)>();

        _repository.SearchForPersonAsync(query.name, Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedList);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Empty(result.PersonDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfPeople_WhenNameContainsSearchTerm()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var returnedPeople = _fixture.Build<Person>()
            .With(p => p.Name, $"NameWhichContains{query.name}SearchTerm")
            .CreateMany()
            .ToList();
        var expectedReturn = (returnedPeople, _fixture.Create<int>());

        _repository.SearchForPersonAsync(query.name, Arg.Any<int>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedReturn);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        foreach (var person in result.PersonDtos)
        {
            Assert.Contains(query.name, person.Name);
        }
    }
}