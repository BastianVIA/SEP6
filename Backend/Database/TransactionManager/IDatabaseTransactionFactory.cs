using Backend.Database.Transaction;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend.Database.TransactionManager;

public interface IDatabaseTransactionFactory
{
    DbTransaction GetCurrentTransaction();
    Task<DbTransaction> BeginTransactionAsync();
    DbReadOnlyTransaction BeginReadOnlyTransaction();
}