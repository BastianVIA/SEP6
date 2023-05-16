using System.ComponentModel.DataAnnotations;
using Backend.User.Infrastructure;
using MediatR;
using NLog;

namespace Backend.User.Application.SetFavoriteMovie;

public record Command(string userId, string movieId) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;

    public CommandHandler(IUserRepository userRepository, IMediator mediator)
    {
        _repository = userRepository;
        _mediator = mediator;
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
            var createUserCommand = new CreateUser.Command(request.userId);
            await _mediator.Send(createUserCommand);
            user = await _repository.ReadUserFromIdAsync(request.userId);
        }

        if (user.HasAlreadyFavoritedMovie(request.movieId))
        {
            user.FavoriteMovies.Remove(request.movieId);
            LogManager.GetCurrentClassLogger()
                .Info($"Movie with id: {request.movieId} removed from favorites of user with id: {request.userId}");
        }
        else
        {
            user.FavoriteMovies.Add(request.movieId);
            LogManager.GetCurrentClassLogger()
                .Info($"User with id: {request.userId} added movie with id: {request.movieId} to favorite list");
        }
        await _repository.Update(user);
    }
}