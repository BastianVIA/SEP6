using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.People.Application.GetPeopleFromId;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using NSubstitute;

namespace TestBackend.People.Application.GetPeopleFromId;

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
    public async Task Handle_ReturnsListOfPeopleWithRightId_WhenIdsExists()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var expectedList = query.personIds.Select(
                id => _fixture.Build<Person>()
                    .With(p => p.Id, id)
                    .Create())
            .ToList();

        _repository.FindPersonsAsync(query.personIds, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedList);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        
        Assert.Equal(query.personIds.Count, result.PersonDtos.Count);
        for (int i = 0; i < expectedList.Count; i++)
        {
            Assert.Equal(expectedList[i].Id, result.PersonDtos[i].ID);
        }
    }
}