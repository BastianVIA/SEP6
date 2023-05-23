using System.Data.Common;
using AutoFixture;
using Backend.Database;
using Backend.Database.Transaction;
using Backend.Database.TransactionManager;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using DbTransaction = Backend.Database.Transaction.DbTransaction;
using NSubstitute;

namespace TestBackend.Util.Mocks;


public class DatabaseTransactionFactoryMock : IDatabaseTransactionFactory
{
    private readonly DbTransaction _transaction;

    public DatabaseTransactionFactoryMock()
    {
        var fixture = new Fixture();
        var transactionSemaphoreMock = fixture.Create<SemaphoreSlim>();
        var dataContextMock = fixture.Create<DataContext>();
        var mediatorMock = fixture.Create<IMediator>();

        _transaction = new DbTransaction(
            transactionSemaphoreMock,
            null, // No need for IDbContextTransaction here, so pass null
            dataContextMock,
            mediatorMock
        );
    }

    public DbTransaction GetCurrentTransaction()
    {
        return _transaction;
    }

    public async Task<DbTransaction> BeginTransactionAsync()
    {
        return _transaction;
    }

    public DbReadOnlyTransaction BeginReadOnlyTransaction()
    {
        return _transaction;
    }
}

