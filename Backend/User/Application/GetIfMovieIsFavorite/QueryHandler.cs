using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetIfMovieIsFavorite;

public record Query(string userId, string movieId) : IRequest<bool>;

public class QueryHandler : IRequestHandler<Query, bool>
{
    private readonly IUserRepository _repository;

    public QueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
    {
        var requestedUser = await _repository.ReadUserFromIdAsync(request.userId);
        return requestedUser.FavoriteMovies.Contains(request.movieId);
    }
}