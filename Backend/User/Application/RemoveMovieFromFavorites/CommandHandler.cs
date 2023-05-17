using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;
using NLog;

namespace Backend.User.Application.RemoveMovieFromFavorites;

public record Command(string userId, string movieId) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;

    public CommandHandler(IUserRepository userRepository, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = userRepository;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _databaseTransactionFactory.BeginTransactionAsync();
        try
        {
            var user = await _repository.ReadUserFromIdAsync(request.userId, transaction);
            if (!user.HasAlreadyFavoritedMovie(request.movieId))
            {
                LogManager.GetCurrentClassLogger()
                    .Error(
                        $"User with id: {request.userId}, tired to remove movid with id: {request.movieId} from favorite list but it is not in the favorite list");
                throw new InvalidDataException($"Movie with Id: {request.movieId} Not In Favorite list");
            }
            user.FavoriteMovies.Remove(request.movieId);
            await _repository.Update(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}