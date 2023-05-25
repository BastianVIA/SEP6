using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using NLog;

namespace Backend.Database.Transaction;

public class DbTransaction : DbReadOnlyTransaction, IAsyncDisposable
{
    public IDbContextTransaction Transaction { get; set; }
    public bool Disposed;
    private readonly IMediator _mediator;
    private List<INotification> _domainEvents;
    private SemaphoreSlim _transactionSemaphore;
    private bool _hasBeenRolledBack;

    public DbTransaction(SemaphoreSlim transactionSemaphore, IDbContextTransaction transaction, DataContext dataContext,
        IMediator mediator) : base(dataContext)
    {
        _transactionSemaphore = transactionSemaphore;
        Transaction = transaction;
        _mediator = mediator;
        _domainEvents = new List<INotification>();
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
            while (_domainEvents.Count > 0)
            {
                // Having the loop like this could cause problems if we have circular events so added debug to find it in case something goes wrong
                LogManager.GetCurrentClassLogger().Debug("Events published"); 
                await pulishDomainEventsAsync(); 

            }
            await DataContext.SaveChangesAsync();
            await Transaction.CommitAsync();
            LogManager.GetCurrentClassLogger().Info("Commited transaction");
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (Disposed || _hasBeenRolledBack)
        {
            return;
        }
        await Transaction.RollbackAsync();
        LogManager.GetCurrentClassLogger().Info("Rolled back transaction");
        _hasBeenRolledBack = true;
    }

    public void AddDomainEvents(List<INotification> domainEvents)
    {
        _domainEvents.AddRange(domainEvents);
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("DIIIIIIIIIIIISPPPPPPPOOOODS");
        if (Disposed)
        {
            return;
        }

        if (!_hasBeenRolledBack)
        {
            await CommitTransactionAsync();
        }

        await Transaction.DisposeAsync();
        Disposed = true;
        _transactionSemaphore.Release();
    }

    private async Task pulishDomainEventsAsync()
    {
        var copiedEvents = _domainEvents.ToList();
        _domainEvents.Clear();
        foreach (var domainEvent in copiedEvents)
        {
            await _mediator.Publish(domainEvent);
        }
        
    }
}