using AutoFixture;
using Backend.Movie.Application.Search;
using Backend.Movie.Infrastructure;
using MediatR;
using NSubstitute;

namespace TestBackend.Movie.Application.Search;

public class QueryHandlerTests
{
    private readonly QueryHandler _sut;
    private readonly IMovieRepository _movieRepository = Substitute.For<IMovieRepository>();
    private readonly Fixture _fixture = new();

    public QueryHandlerTests()
    {
        _sut = new QueryHandler(_movieRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoResultFound()
    {
        //Arrange
        var request = new Query("MovieNameDoesNotExist");
        var expected = new List<Backend.Movie.Domain.Movie>();
        _movieRepository.SearchForMovie(request.Title).Returns(expected);
        
        //Act
        var result = await _sut.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result.movieDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfMovies_WhenResultFound()
    {
        //Arrange
        var expectedMovie = _fixture.Create<Backend.Movie.Domain.Movie>();
        var returnedMoveList = new List<Backend.Movie.Domain.Movie>
        {
            new()
            {
                Id = expectedMovie.Id,
                ReleaseYear = expectedMovie.ReleaseYear,
                Title = expectedMovie.Title
            }
        };
        var request = new Query(Arg.Any<string>());
        _movieRepository.SearchForMovie(request.Title).Returns(returnedMoveList);
        //Act
        var result = await _sut.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.movieDtos);
        Assert.Equal(expectedMovie.Id, result.movieDtos[0].Id);
        Assert.Equal(expectedMovie.ReleaseYear, result.movieDtos[0].ReleaseYear);
        Assert.Equal(expectedMovie.Title, result.movieDtos[0].Title);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfMovies_WhenTitleIsPartialMatch()
    {
        //Arrange 
        var partialTitle = _fixture.Create<string>();
        var returnedMovies = _fixture.CreateMany<Backend.Movie.Domain.Movie>(3)
            .Select(m => new Backend.Movie.Domain.Movie
            {
                Id = m.Id,
                ReleaseYear = m.ReleaseYear,
                Title = m.Title + partialTitle
            }).ToList();

        var request = new Query(partialTitle);
        _movieRepository.SearchForMovie(request.Title).Returns(returnedMovies);
        
        //Act
        var result = await _sut.Handle(request, CancellationToken.None);
        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.movieDtos);
        Assert.Equal(returnedMovies[0].Title, result.movieDtos[0].Title);
        Assert.Equal(returnedMovies[1].Title, result.movieDtos[1].Title);
        Assert.Equal(returnedMovies[2].Title, result.movieDtos[2].Title);
    }

}