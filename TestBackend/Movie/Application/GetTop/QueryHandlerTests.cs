using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetTop100;
using Backend.Movie.Domain;
using Backend.Movie.Infrastructure;
using Backend.Service;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using TestBackend.Util;

namespace TestBackend.Movie.Application.GetTop;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IConfiguration _configuration = GetConfig.GetTestConfig();
    private readonly IMovieRepository _repository = Substitute.For<IMovieRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_configuration, _repository, _imageService, _transactionFactory);
    }
    

    [Fact]
    public async Task Handle_ShouldReturnListOfMoviesWithMinimumVotes_WhenCalled()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var minVotes = _configuration.GetSection("QueryConstants").GetValue<int>("MinVotes");

        var expectedMovies = _fixture.Build<Backend.Movie.Domain.Movie>()
            .With(movie => movie.Rating, _fixture.Build<Rating>()
                .With(rating => rating.Votes, new Random().Next(minVotes +1, 10000))
                .Create())
            .CreateMany()
            .ToList();
        var expectedPoster = _fixture.Create<Uri>();

        _repository.GetTopMoviesAsync(minVotes, query.PageNumber, Arg.Any<DbReadOnlyTransaction>())
            .Returns(expectedMovies);
        _imageService.GetPathForPosterAsync(Arg.Any<string>()).Returns(expectedPoster);
        
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        foreach (var movie in result.topMovies)
        {
            Assert.True(movie.Rating.Votes > minVotes);
        }
    }


}