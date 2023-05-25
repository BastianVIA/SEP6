using Backend.Database.Transaction;
using MediatR;

namespace Backend.Database.TransactionManager;

public class DatabaseTransactionFactory : IDatabaseTransactionFactory
{
    private readonly DataContext _dbContext;
    private DbTransaction? _currentTransaction;
    private readonly SemaphoreSlim _transactionSemaphore;
    private readonly IMediator _mediator;

    public DatabaseTransactionFactory(DataContext dbContext, SemaphoreSlim transactionSemaphore, IMediator mediator)
    {
        _dbContext = dbContext;
        _transactionSemaphore = transactionSemaphore;
        _mediator = mediator;
    }

    public DbTransaction GetCurrentTransaction()
    {
        if (_currentTransaction == null || _currentTransaction.Disposed)
        {
            throw new ApplicationException(
                "Trying to get transaction, but it has already been Disposed or never begun");
        }
        return _currentTransaction;
    }

    public async Task<DbTransaction> BeginTransactionAsync()
    {
        await _transactionSemaphore.WaitAsync();

        if (_currentTransaction != null && !_currentTransaction.Disposed)
        {
            _transactionSemaphore.Release();
            return null;
        }

        var dbTransaction = await _dbContext.Database.BeginTransactionAsync();
        _currentTransaction = new DbTransaction(_transactionSemaphore, dbTransaction, _dbContext, _mediator);
        return _currentTransaction;
    }

    public DbReadOnlyTransaction BeginReadOnlyTransaction()
    {
        return new DbReadOnlyTransaction(_dbContext);
    }
}