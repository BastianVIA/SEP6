using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.User.Application.RateMovie;
using Backend.User.Domain;
using Backend.User.Infrastructure;
using NSubstitute;

namespace TestBackend.User.Application.RateMovie;

public class CommandHandlerTests
{
    private CommandHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();
    
    private const int RatingMax = 10;
    private const int RatingMin = 1;
    public CommandHandlerTests()
    {
        _handler = new CommandHandler(_repository, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReadUserIncludesRatings()
    {
        var request = CreateValidCommandWithRating();
        var user = _fixture.Create<Backend.User.Domain.User>();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbTransaction>(), includeRatings:Arg.Any<bool>(), includeFavoriteMovies:Arg
                .Any<bool>())
            .Returns(user);

        await _handler.Handle(request, CancellationToken.None);

        await _repository.Received(1).ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbTransaction>(),
            includeRatings: true, includeFavoriteMovies: Arg.Any<bool>());
    }
    
    [Fact]
    public async Task Handle_UpdatesUser_WhenMovieNotAlreadyRated()
    {
        var request = CreateValidCommandWithRating();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Ratings, _fixture.Create<List<UserRating>>())
            .Create();

        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbTransaction>(), includeRatings:Arg.Any<bool>(), includeFavoriteMovies:Arg
                .Any<bool>())
            .Returns(user);

        await _handler.Handle(request, CancellationToken.None);

        await _repository.Received(1).UpdateAsync(user, Arg.Any<DbTransaction>());
    }

    [Fact]
    public async Task Handle_UpdatesUser_WhenMovieAlreadyRated()
    {
        var request = CreateValidCommandWithRating();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.Ratings, new List<UserRating>
            {
                new(request.movieId, RandomStar())
            })
            .Create();
        
        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbTransaction>(), includeRatings:Arg.Any<bool>(), includeFavoriteMovies:Arg
                .Any<bool>())
            .Returns(user);

        await _handler.Handle(request, CancellationToken.None);

        await _repository.Received(1).UpdateAsync(user, Arg.Any<DbTransaction>());
    }

    private Command CreateValidCommandWithRating()
    {
        return _fixture.Build<Command>()
            .With(c => c.rating, RandomStar())
            .Create();
    }

    private int RandomStar()
    {
        return _fixture.Create<int>() % RatingMax + RatingMin;
    }
}