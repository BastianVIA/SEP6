using Backend.Database.TransactionManager;
using Backend.Service;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.SetUserImage;

public record Command(string userId, byte[] data) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly IUserImageRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public CommandHandler( IUserImageRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _transactionFactory.BeginTransactionAsync();
        try
        {
            await _repository.UploadImageAsync(request.userId, request.data, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}