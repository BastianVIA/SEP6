using Backend.Database.TransactionManager;
using Backend.Service;
using Backend.User.Infrastructure;
using MediatR;

namespace Backend.User.Application.GetUserImage;

public record Query(string userId) : IRequest<UserImageResponse>;

public record UserImageResponse(UserImageDto UserImageDto);

public class UserImageDto
{
    public byte[]? ImageData { get; set; }
}

public class QueryHandler : IRequestHandler<Query, UserImageResponse>
{
    private readonly IUserImageRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public QueryHandler( IUserImageRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task<UserImageResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.BeginReadOnlyTransaction();
        var image = await _repository.ReadImageForUserAsync(request.userId, transaction);

        return new UserImageResponse(new UserImageDto(){ImageData = image});
    }

}