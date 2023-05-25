using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.CreateReview;

public record Command(string userId, string movieId, string reviewBody) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;
   
    public CommandHandler(IUserRepository userRepository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = userRepository;
        _transactionFactory = transactionFactory;
    }
    
    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _transactionFactory.BeginTransactionAsync();

        try
        {
            var user = await _repository.ReadUserFromIdAsync(request.userId, transaction, includeReviews: true);
            user.AddReview(request.movieId, request.reviewBody);

            await _repository.UpdateAsync(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
        

    }
}