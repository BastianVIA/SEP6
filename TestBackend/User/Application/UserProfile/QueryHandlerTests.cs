using AutoFixture;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using Backend.Movie.Application.GetMovieInfo;
using Backend.User.Domain;
using Backend.User.Infrastructure;
using MediatR;
using NSubstitute;
using Query = Backend.User.Application.UserProfile.Query;
using QueryHandler = Backend.User.Application.UserProfile.QueryHandler;

namespace TestBackend.User.Application.UserProfile;

public class QueryHandlerTests
{
    private QueryHandler _handler;
    private Fixture _fixture = new();
    private readonly IUserRepository _repository = Substitute.For<IUserRepository>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IDatabaseTransactionFactory _transactionFactory = Substitute.For<IDatabaseTransactionFactory>();

    public QueryHandlerTests()
    {
        _handler = new QueryHandler(_repository, _mediator, _transactionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReadUserIncludingRatingsAndFavourites()
    {
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.FavoriteMovies, _fixture.Create<List<UserFavoriteMovie>>())
            .With(u => u.Ratings, _fixture.Build<UserRating>()
                .With(r => r.NumberOfStars, GetRandomStar())
                .CreateMany()
                .ToList())
            .Create();
        var movieInfoResponse = _fixture.Create<GetMovieInfoResponse>();
        
        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(user);
        _mediator.Send(Arg.Any<Backend.Movie.Application.GetMovieInfo.Query>())
            .Returns(movieInfoResponse);

       await _handler.Handle(query, CancellationToken.None);

       await _repository.Received(1).ReadUserFromIdAsync(query.userId, Arg.Any<DbReadOnlyTransaction>(), true, true);
    }

    [Fact]
    public async Task Handle_ReturnsCorrectUser_WhenIdFound()
    {
        var query = _fixture.Create<Query>();
        var user = _fixture.Build<Backend.User.Domain.User>()
            .With(u => u.FavoriteMovies, _fixture.Create<List<UserFavoriteMovie>>())
            .With(u => u.Ratings, _fixture.Build<UserRating>()
                .With(r => r.NumberOfStars, GetRandomStar())
                .CreateMany()
                .ToList())
            .Create();
        var movieInfoResponse = _fixture.Create<GetMovieInfoResponse>();
        
        _repository.ReadUserFromIdAsync(Arg.Any<string>(), Arg.Any<DbReadOnlyTransaction>(), Arg.Any<bool>(), Arg.Any<bool>())
            .Returns(user);
        _mediator.Send(Arg.Any<Backend.Movie.Application.GetMovieInfo.Query>())
            .Returns(movieInfoResponse);

        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.Equal(user.DisplayName, result.userProfile.DisplayName);
        
    }

    private int GetRandomStar()
    {
        return _fixture.Create<int>() % 10 + 1;
    }
}