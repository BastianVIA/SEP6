using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.NumberOfResultsForSearch;

public record Query(string DisplayName) : IRequest<int>;

public class QueryHandler : IRequestHandler<Query, int>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler(IUserRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<int> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        return await _repository.NumberOfResultsForSearch(request.DisplayName, transaction);    }
}