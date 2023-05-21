using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetRecommendations;
using Backend.Movie.Infrastructure;
using Backend.Service;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace TestBackend.Movie.Application.GetRecommendations;

public class QueryHandlerTest
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IConfiguration _configuration = Substitute.For<IConfiguration>();
    private readonly IMovieRepository _repository = Substitute.For<IMovieRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    
    public QueryHandlerTest()
    {
        _handler = new QueryHandler(_configuration,_repository, _imageService, _transactionFactory);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnListOfMovies()
    {
        //Arrange 
        var query = new Query();
        var numberOfMoviesGenerated = 3;
        var expectedMovies = _fixture.CreateMany<Backend.Movie.Domain.Movie>(numberOfMoviesGenerated).ToList();

        _repository.GetRecommendedMovies(Arg.Any<int>(), Arg.Any<float>(), Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedMovies);
        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.NotEmpty(result.MovieRecommendations);
        Assert.Equal(numberOfMoviesGenerated, result.MovieRecommendations.Count);
        assertMoviesEqual(expectedMovies[0], result.MovieRecommendations[0]);
        assertMoviesEqual(expectedMovies[1], result.MovieRecommendations[1]);
        assertMoviesEqual(expectedMovies[2], result.MovieRecommendations[2]);

    }
    private void assertMoviesEqual(Backend.Movie.Domain.Movie expectedMovie, MovieRecommendationDto actualMovie)
    {
        Assert.Equal(expectedMovie.Id, actualMovie.Id);
        Assert.Equal(expectedMovie.Title, actualMovie.Title);
        Assert.Equal(expectedMovie.ReleaseYear, actualMovie.ReleaseYear);
        if (expectedMovie.Rating != null)
        {
            Assert.Equal(expectedMovie.Rating.AverageRating, actualMovie.Rating.AverageRating);
            Assert.Equal(expectedMovie.Rating.Votes, actualMovie.Rating.Votes);
            
        }
        
    }

}