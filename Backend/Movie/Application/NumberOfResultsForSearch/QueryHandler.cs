using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using MediatR;

namespace Backend.Movie.Application.NumberOfResultsForSearch;

public record Query(string Title) : IRequest<int>;

public class QueryHandler : IRequestHandler<Query, int>
{
    private readonly IMovieRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler(IMovieRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<int> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        return await _repository.NumberOfResultsForSearch(request.Title, transaction);
    }
}