using Backend.Database.Transaction;

namespace Backend.Database.TransactionManager;

public interface IDatabaseTransactionFactory
{
    DbTransaction GetCurrentTransaction();
    Task<DbTransaction> BeginTransactionAsync();
    DbReadOnlyTransaction BeginReadOnlyTransaction();
}