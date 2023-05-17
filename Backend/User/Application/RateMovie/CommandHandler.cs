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
            var user = await _repository.ReadUserFromIdAsync(request.userId, transaction, includeRatings:true);
            if (user.HasAlreadyRatedMovie(request.movieId))
            {
                if (request.rating == null)
                {
                    user.RemoveRating(request.movieId);
                }
                else
                {
                    user.UpdateRating(request.movieId, (int)request.rating);
                }
            }
            else
            {
                if (request.rating == null)
                {
                    throw new ValidationException(
                        $"User with id: {request.userId} has already rated movie with id: {request.movieId}");
                }

                user.AddRating(request.movieId, (int)request.rating);
            }
            await _repository.Update(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    
        
        
    }
}