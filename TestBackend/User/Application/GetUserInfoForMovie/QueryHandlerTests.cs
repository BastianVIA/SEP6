using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Domain;
using Backend.User.Application.GetUserInfoForMovie;
using Backend.User.Domain;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.GetUserInfoForMovie;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnGetUserInfoMovieResponse()
    {
        
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Ratings, _fixture.Create<List<UserRating>>())
            .With(u => u.FavoriteMovies, _fixture.Create<List<UserFavoriteMovie>>())
            .Create();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), includeRatings:Arg.Any<bool>(), includeFavoriteMovies:Arg
                .Any<bool>())
            .Returns(user);
        
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.IsType<GetUserInfoForMovieResponse>(result);
    }

    [Fact]
    public async Task Handle_ReadUserIncludesRatingsAndFavourites()
    {
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Ratings, _fixture.Create<List<UserRating>>())
            .With(u => u.FavoriteMovies, _fixture.Create<List<UserFavoriteMovie>>())
            .Create();
        
        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), includeRatings:Arg.Any<bool>(), includeFavoriteMovies:Arg
                .Any<bool>())
            .Returns(user);

        await _handler.Handle(query, CancellationToken.None);

        await _repository.Received(1).ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(),
            includeRatings: true, includeFavoriteMovies: true);
    }

    [Fact]
    public async Task Handle_ReturnsWithIsFavouriteTrue_WhenMovieIsFavourited()
    {
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Ratings, _fixture.Create<List<UserRating>>())
            .With(u => u.FavoriteMovies, new List<UserFavoriteMovie>
            {
                new(query.MovieId)
            })
            .Create();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>(),
                includeFavoriteMovies: true)
            .Returns(user);

        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.True(result.IsFavorite);
    }

    [Fact]
    public async Task Handle_ReturnsWithRating_WhenUserHasRatingForMovie()
    {
        var query = _fixture.Create<Query>();
        var expectedRating = 4;
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.FavoriteMovies, _fixture.Create<List<UserFavoriteMovie>>())
            .With(u => u.Ratings, new List<UserRating>
            {
                new(query.MovieId, expectedRating)
            })
            .Create();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), includeRatings: Arg.Any<bool>(),
                includeFavoriteMovies: Arg.Any<bool>())
            .Returns(user);

        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(expectedRating, result.NumberOfStars);
    }

    [Fact]
    public async Task Handle_ReturnsNullStars_WhenUserHasNoRatingForMovie()
    {
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Ratings, _fixture.Create<List<UserRating>>())
            .With(u => u.FavoriteMovies, _fixture.Create<List<UserFavoriteMovie>>())
            .Create();
        
        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), includeRatings: Arg.Any<bool>(),
                includeFavoriteMovies: Arg.Any<bool>())
            .Returns(user);

        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Null(result.NumberOfStars);
    }
}