using Microsoft.EntityFrameworkCore.Storage;
using NLog;

namespace Backend.Database.Transaction;

public class DbTransaction : DbReadOnlyTransaction, IAsyncDisposable
{
    public IDbContextTransaction Transaction { get; set; }
    public bool Disposed;
    private SemaphoreSlim _transactionSemaphore;
    private bool commited;
    
    public DbTransaction(SemaphoreSlim transactionSemaphore, IDbContextTransaction transaction, DataContext dataContext) : base(dataContext)
    {
        _transactionSemaphore = transactionSemaphore;
        Transaction = transaction;
    }
    async Task CommitTransactionAsync()
    {
        if (Disposed)
        {
            return;
        }
        if (Transaction == null)
        {
            throw new ArgumentNullException(nameof(Transaction));
        }

        try
        {
            await DataContext.SaveChangesAsync();
            await Transaction.CommitAsync();
            commited = true;

        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (commited)
        {
            return;
        }
        await Transaction.RollbackAsync();
        

    }

    public async ValueTask DisposeAsync()
    {
        if (!commited)
        {
            await CommitTransactionAsync();

        }
        await Transaction.DisposeAsync();
        _transactionSemaphore.Release();
        
    }
}