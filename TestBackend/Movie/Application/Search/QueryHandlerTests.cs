using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Enum;
using Backend.Movie.Application.Search;
using Backend.Movie.Infrastructure;
using Backend.Service;
using NSubstitute;

namespace TestBackend.Movie.Application.Search;

public class QueryHandlerTests
{
    private readonly QueryHandler _handler;
    private readonly IMovieRepository _movieRepository = Substitute.For<IMovieRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly Fixture _fixture = new();
    
    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_movieRepository, _imageService, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoResultFound()
    {
        //Arrange
        var request = _fixture.Create<Query>();
        var expected = new List<Backend.Movie.Domain.Movie>();
        _movieRepository.SearchForMovie(request.Title, request.sortingKey, request.sortingDirection, request.pageNumber, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expected);
        
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result.MovieDtos);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnOneMovie_WhenOneMatchFound()
    {
        //Arrange 
        var returnedMovie = _fixture.Create<Backend.Movie.Domain.Movie>();
        var expected = new List<Backend.Movie.Domain.Movie> { returnedMovie };
        var request = _fixture.Create<Query>();
        _movieRepository.SearchForMovie(request.Title, 
                Arg.Any<MovieSortingKey>(), 
                Arg.Any<SortingDirection>(), 
                Arg.Any<int>(), 
                Arg.Any<DbReadOnlyTransaction>())
            .Returns(expected);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);
        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.MovieDtos);
        Assert.Equal(returnedMovie.Title, result.MovieDtos[0].Title);
        Assert.Equal(returnedMovie.Rating.Votes, result.MovieDtos[0].Rating.Votes);
        Assert.Equal(returnedMovie.Rating.AverageRating, result.MovieDtos[0].Rating.AverageRating);
        Assert.Equal(returnedMovie.Id, result.MovieDtos[0].Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfMovies_WhenManyMoviesMatchSearchedTitle()
    {
        //Arrange 
        var returnedMovies = _fixture.CreateMany<Backend.Movie.Domain.Movie>(3).ToList();
        var request = _fixture.Create<Query>();
        _movieRepository.SearchForMovie(request.Title, 
                Arg.Any<MovieSortingKey>(), 
                Arg.Any<SortingDirection>(), 
                Arg.Any<int>(), 
                Arg.Any<DbReadOnlyTransaction>())
            .Returns(returnedMovies);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);
        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.MovieDtos);
        Assert.Equal(returnedMovies[0].Title, result.MovieDtos[0].Title);
        Assert.Equal(returnedMovies[1].Title, result.MovieDtos[1].Title);
        Assert.Equal(returnedMovies[2].Title, result.MovieDtos[2].Title);
    }
    
}