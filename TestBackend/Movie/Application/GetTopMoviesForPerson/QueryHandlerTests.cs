using System.Runtime.InteropServices.JavaScript;
using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetTopMoviesForPerson;
using Backend.Movie.Infrastructure;
using Backend.Service;
using NSubstitute;

namespace TestBackend.Movie.Application.GetTopMoviesForPerson;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IMovieRepository _repository = Substitute.For<IMovieRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_transactionFactory, _repository, _imageService);
    }


    [Fact]
    public async Task Handle_ShouldReturnTopMoviesActedAndDirected_OnValidRequest()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var expectedActedList = _fixture.CreateMany<Backend.Movie.Domain.Movie>().ToList();
        var expectedDirectedList = _fixture.CreateMany<Backend.Movie.Domain.Movie>().ToList();

        _repository.GetActedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedActedList);
        _repository.GetDirectedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedDirectedList);
        
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        foreach (var movie in result.ActedMovies!)
        {
            Assert.Contains(expectedActedList, m => m.Id.Equals(movie.MovieId));
        }

        foreach (var movie in result.DirectedMovies!)
        {
            Assert.Contains(expectedDirectedList, m => m.Id.Equals(movie.MovieId));
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnNullDirectedList_WhenNoDirectedMoviesFound()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var expectedActedList = _fixture.CreateMany<Backend.Movie.Domain.Movie>().ToList();
        List<Backend.Movie.Domain.Movie>? expectedDirectedList = null;

        _repository.GetActedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedActedList);
        _repository.GetDirectedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedDirectedList);
        
        
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        foreach (var movie in result.ActedMovies!)
        {
            Assert.Contains(expectedActedList, m => m.Id.Equals(movie.MovieId));
        }

        Assert.Null(result.DirectedMovies);
    }
    [Fact]
    public async Task Handle_ShouldReturnNullActedList_WhenNoActedMoviesFound()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        List<Backend.Movie.Domain.Movie>? expectedActedList = null;
        var expectedDirectedList = _fixture.CreateMany<Backend.Movie.Domain.Movie>().ToList();

        _repository.GetActedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedActedList);
        _repository.GetDirectedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedDirectedList);
        
        
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        foreach (var movie in result.DirectedMovies!)
        {
            Assert.Contains(expectedDirectedList, m => m.Id.Equals(movie.MovieId));
        }

        Assert.Null(result.ActedMovies);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnNullLists_WhenNoActedAndDirectedMoviesFound()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        List<Backend.Movie.Domain.Movie>? expectedActedList = null;
        List<Backend.Movie.Domain.Movie>? expectedDirectedList = null;

        _repository.GetActedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedActedList);
        _repository.GetDirectedMoviesForPersonAsync(query.PersonId, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedDirectedList);
        
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.Null(result.DirectedMovies);
        Assert.Null(result.ActedMovies);
    }
}