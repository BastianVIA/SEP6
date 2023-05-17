using System.ComponentModel.DataAnnotations;
using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.RateMovie;

public record Command(string userId, string movieId, int? rating) : IRequest;


public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IMediator _mediator;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public CommandHandler(IUserRepository repository, IMediator mediator, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _mediator = mediator;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _transactionFactory.BeginTransactionAsync();

        try
        {
            var user = await _repository.ReadUserWithFavouriteMoviesFromIdAsync(request.userId, transaction);
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
            await _repository.Update(user, transaction);

            foreach (var domainEvent in user.ReadAllDomainEvents())
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
        
        
    }
}