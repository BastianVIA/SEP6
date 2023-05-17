using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetUserInfoForMovie;

public record Query(string UserId, string MovieId) : IRequest<GetUserInfoForMovieResponse>;

public record GetUserInfoForMovieResponse(bool IsFavorite, int? NumberOfStars);

public class QueryHandler : IRequestHandler<Query, GetUserInfoForMovieResponse>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;
    public QueryHandler(IUserRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<GetUserInfoForMovieResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var user = await _repository.ReadUserFromIdAsync(request.UserId, transaction, includeRatings:true, includeFavoriteMovies:true);
        int? numberOfStars = null;
        if (user.HasAlreadyRatedMovie(request.MovieId))
        {
            numberOfStars = user.GetRatingForMovie(request.MovieId);
        }

        return new GetUserInfoForMovieResponse(user.HasAlreadyFavoritedMovie(request.MovieId), numberOfStars);
    }
}