using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetInfoFromMovies;
using Backend.User.Infrastructure;
using MediatR;
using NSubstitute;
using Query = Backend.User.Application.GetFavoriteMovies.Query;
using QueryHandler = Backend.User.Application.GetFavoriteMovies.QueryHandler;

namespace TestBackend.User.Application.GetFavoriteMovies;

public class QueryHandlerTest
{
    private readonly QueryHandler _handler;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    private readonly Fixture _fixture = new();

    public QueryHandlerTest()
    {
        _handler = new QueryHandler(_userRepository, _mediator, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoMoviesFavorited()
    {
        //Arrange
        var request = _fixture.Create<Query>();
        var expectedUser = new Backend.User.Domain.User
        {
            Id = "realId",
            FavoriteMovies = _fixture.Create<List<string>>()
        };
        var expectedMovies = new MoviesInfoResponse(new List<MovieInfoDto>());
        _userRepository.ReadUserFromIdAsync(Arg.Any<string>(),Arg.Any<DbReadOnlyTransaction>(), includeFavoriteMovies:true).Returns(expectedUser);
        _mediator.Send(Arg.Any<Backend.Movie.Application.GetInfoFromMovies.Query>()).Returns(expectedMovies);
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        Assert.Empty(result.movies);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnListOfMovies_WhenUserHasFavoritedMovies()
    {
        //Arrange
        var request = _fixture.Create<Query>();
        var listOfFavoriteMovies = new List<string>() { "movie1", "movie2" };
        var moviesToReturn = _fixture.CreateMany<MovieInfoDto>(2).ToList();
        var expectedResponse = new MoviesInfoResponse(moviesToReturn);
        var expectedUser = new Backend.User.Domain.User
        {
            Id = "realId",
            FavoriteMovies = listOfFavoriteMovies
        };
        _userRepository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), includeFavoriteMovies:true).Returns(expectedUser);
        _mediator.Send(Arg.Any<Backend.Movie.Application.GetInfoFromMovies.Query>()).Returns(expectedResponse);
        //Act
        var result = await _handler.Handle(request, CancellationToken.None);
        
        //Assert
        Assert.NotEmpty(result.movies);
        Assert.Equal(listOfFavoriteMovies.Count, result.movies.Count);
        Assert.Equal(expectedResponse.MovieInfoDtos[0].Id, result.movies[0].Id);
        Assert.Equal(expectedResponse.MovieInfoDtos[0].Title, result.movies[0].Title);
        Assert.Equal(expectedResponse.MovieInfoDtos[0].ReleaseYear, result.movies[0].ReleaseYear);
        Assert.Equal(expectedResponse.MovieInfoDtos[0].PathToPoser, result.movies[0].PathToPoster);
        if (expectedResponse.MovieInfoDtos[0].Rating != null)
        {
            Assert.Equal(expectedResponse.MovieInfoDtos[0].Rating.Votes, result.movies[0].Rating.Votes);
            Assert.Equal(expectedResponse.MovieInfoDtos[0].Rating.AverageRating, result.movies[0].Rating.AverageRating);
            
        }
    }
    
}