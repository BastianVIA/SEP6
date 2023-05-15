using Backend.User.Infrastructure;
using MediatR;
using NLog;

namespace Backend.User.Application.RemoveMovieFromFavorites;
public record Command(string userId, string movieId) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;

    public CommandHandler(IUserRepository userRepository)
    {
        _repository = userRepository;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.User user;
        try
        {
            user = await _repository.ReadUserFromIdAsync(request.userId);
        }
        catch (InvalidOperationException e)
        {
            LogManager.GetCurrentClassLogger().Error($"Tried to remove movie with id: {request.movieId} from user with id: {request.userId}, but could not find user");
            throw new InvalidDataException($"Could not find user with id {request.userId}");
        }

        if (!user.HasAlreadyFavoritedMovie(request.movieId))
        {
            LogManager.GetCurrentClassLogger()
                .Error(
                    $"User with id: {request.userId}, tired to remove movid with id: {request.movieId} from favorite list but it is not in the favorite list");
            throw new InvalidDataException($"Movie with Id: {request.movieId} Not In Favorite list");
        }
  
        user.FavoriteMovies.Remove(request.movieId);
        await _repository.Update(user);
    }
}