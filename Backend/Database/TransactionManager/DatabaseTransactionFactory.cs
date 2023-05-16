using Backend.Database.Transaction;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;

namespace Backend.Database.TransactionManager;

public class DatabaseTransactionFactory : IDatabaseTransactionFactory
{
    private readonly DataContext _dbContext;
    private DbTransaction? _currentTransaction;
    private readonly SemaphoreSlim _transactionSemaphore;

    public DatabaseTransactionFactory(DataContext dbContext, SemaphoreSlim transactionSemaphore)
    {
        _dbContext = dbContext;
        _transactionSemaphore = transactionSemaphore;
    }

    public DbTransaction? GetCurrentTransaction()
    {
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
        _currentTransaction = new DbTransaction(_transactionSemaphore, dbTransaction, _dbContext);
        return _currentTransaction;
    }

    public DbReadOnlyTransaction BeginReadOnlyTransaction()
    {
        return new DbReadOnlyTransaction(_dbContext);
    }
}