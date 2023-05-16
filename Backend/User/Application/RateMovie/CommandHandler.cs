using System.ComponentModel.DataAnnotations;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.RateMovie;

public record Command(string userId, string movieId, int? rating) : IRequest;


public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;

    public CommandHandler(IUserRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        
        var user = await _repository.ReadUserWithRatingsFromIdAsync(request.userId);
        if (request.rating == null && !user.HasAlreadyRatedMovie(request.movieId))
        {
            throw new ValidationException(
                $"User with id: {request.userId} has already rated movie with id: {request.movieId}");    
        }
        if (user.HasAlreadyRatedMovie(request.movieId))
        {
            user.RemoveRating(request.movieId);
        }
        else
        {
            user.AddRating(request.movieId, (int)request.rating);
        }
        await _repository.Update(user);

        foreach (var domainEvent in user.ReadAllDomainEvents())
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
        
    }
}