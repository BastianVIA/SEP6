using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetIfMovieIsFavorite;

public record Query(string userId, string movieId) : IRequest<bool>;

public class QueryHandler : IRequestHandler<Query, bool>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public QueryHandler(IUserRepository repository, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = repository;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _databaseTransactionFactory.BeginReadOnlyTransaction();
        var requestedUser = await _repository.ReadUserFromIdAsync(request.userId, transaction, includeFavoriteMovies:true);
        return requestedUser.HasAlreadyFavoritedMovie(request.movieId);
    }
}