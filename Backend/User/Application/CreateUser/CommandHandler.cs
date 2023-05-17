using System.ComponentModel.DataAnnotations;
using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.CreateUser;

public record Command(string userId, string displayName, string email) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _databaseTransactionFactory;
   
    public CommandHandler(IUserRepository userRepository, IMediator mediator, IDatabaseTransactionFactory databaseTransactionFactory)
    {
        _repository = userRepository;
        _databaseTransactionFactory = databaseTransactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
       await using var transaction = await _databaseTransactionFactory.BeginTransactionAsync();
       try
       {
           await _repository.CreateUserAsync(request.userId, request.displayName, request.email, transaction);
       }
       catch (Exception e)
       {
           await transaction.RollbackTransactionAsync();
           throw;
       }
    }
}