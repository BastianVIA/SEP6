using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetTopMoviesForPerson;
using Backend.People.Domain;
using Backend.People.Infrastructure;
using Backend.Service;
using MediatR;
using NSubstitute;
using Query = Backend.People.Application.GetDetails.Query;
using QueryHandler = Backend.People.Application.GetDetails.QueryHandler;

namespace TestBackend.People.Application.GetDetail;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IPeopleRepository _repository = Substitute.For<IPeopleRepository>();
    private readonly IPersonService _personService = Substitute.For<IPersonService>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_personService, _transactionFactory, _repository, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldReturnPersonDetailsWithActedAndDirected_WhenIdFound()
    {
        // Arrange
        
        var query = _fixture.Create<Query>();
        var expectedDetails = _fixture.Build<Person>()
            .With(p => p.Id, query.PersonId)
            .With(p => p.DirectedMoviesId, _fixture.Create<List<string>>())
            .With(p => p.ActedMoviesId, _fixture.Create<List<string>>())
            .Create();
        var expectedPersonServiceDto = _fixture.Create<Task<PersonServiceDto?>>();
        var moviesResponse = _fixture.Create<Task<GetTopMoviesForPersonResponse>>();
        

        _repository.ReadPersonFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(expectedDetails);
        _personService.GetPersonAsync(Arg.Any<string>())
            .Returns(expectedPersonServiceDto);
        _mediator.Send(Arg.Is(new Backend.Movie.Application.GetTopMoviesForPerson.Query(query.PersonId)),
                CancellationToken.None)
            .Returns(moviesResponse);
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        
        Assert.Equal(expectedDetails.Name, result.Name);
        Assert.Equal(expectedDetails.BirthYear, result.BirthYear);
        Assert.Equal(expectedDetails.Id, result.Id);
        Assert.Equal(expectedPersonServiceDto.Result.KnownFor, result.KnownFor);
        Assert.Equal(expectedPersonServiceDto.Result.Bio, result.Bio);
        Assert.Equal(moviesResponse.Result.ActedMovies.Count, result.ActedMovies.Count);
        Assert.Equal(moviesResponse.Result.DirectedMovies.Count, result.DirectedMovies.Count);
        
    }
}