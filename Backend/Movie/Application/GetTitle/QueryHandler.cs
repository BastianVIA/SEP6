using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using MediatR;

namespace Backend.Movie.Application.GetTitle;

public record Query(string MovieId) : IRequest<string>;

public class QueryHandler : IRequestHandler<Query, string>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IMovieRepository _repository;

    public QueryHandler(IDatabaseTransactionFactory transactionFactory, IMovieRepository repository)
    {
        _transactionFactory = transactionFactory;
        _repository = repository;
    }

    public async Task<string> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var movie = await _repository.ReadMovieFromIdAsync(request.MovieId, transaction);
        return movie.Title;
    }
}