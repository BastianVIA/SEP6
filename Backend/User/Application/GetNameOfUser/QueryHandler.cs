using Backend.Database.TransactionManager;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetNameOfUser;

public record Query(string Id) : IRequest<string>;


public class QueryHandler : IRequestHandler<Query, string>
{
    private readonly IUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;
    
    public QueryHandler(IUserRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<string> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var user = await _repository.ReadUserFromIdAsync(request.Id, transaction);
        return user.DisplayName;
    }
}