using Backend.Database.TransactionManager;
using Backend.People.Infrastructure;
using MediatR;

namespace Backend.People.Application.NumberOfResultsForSearch;

public record Query(string Name) : IRequest<int>;


public class QueryHandler : IRequestHandler<Query, int>
{
    private readonly IPeopleRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler(IPeopleRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<int> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        return await _repository.NumberOfResultsForSearch(request.Name, transaction);
    }
}