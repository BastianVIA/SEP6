using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.User.Application.GetIfMovieIsFavorite;
using Backend.User.Domain;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.GetIfMovieIsFavorite;

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
    public async Task Handle_ShouldReturnTrue_WhenMovieIsFavourited()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.FavoriteMovies, new List<UserFavoriteMovie>
            {
                new(query.movieId)
            })
            .Create();

        _repository.ReadUserFromIdAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), includeFavoriteMovies: true)
            .Returns(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenMovieIsNotFavourited()
    {
        // Arrange
        var query = _fixture.Create<Query>();
        var user = _fixture.Create<Backend.User.Domain.User>();
        
        _repository.ReadUserFromIdAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), includeFavoriteMovies: true)
            .Returns(user);
        // Act

        var result = await _handler.Handle(query, CancellationToken.None);
        // Assert
        Assert.False(result);
    }
}