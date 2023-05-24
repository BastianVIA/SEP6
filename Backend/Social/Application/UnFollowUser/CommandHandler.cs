using System.ComponentModel.DataAnnotations;
using Backend.Database.TransactionManager;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.Social.Application.UnFollowUser;

public record Command(string UserId, string UserToUnFollow) : IRequest;

public class CommandHandler : IRequestHandler<Command>
{
    private readonly ISocialUserRepository _repository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public CommandHandler(ISocialUserRepository repository, IDatabaseTransactionFactory transactionFactory)
    {
        _repository = repository;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(Command request, CancellationToken cancellationToken)
    {
        await using var transaction = await _transactionFactory.BeginTransactionAsync();
        try
        {
            var user = await _repository.ReadSocialUserAsync(request.UserId, transaction, includeFollowing:true);
            user.UnFollow(request.UserToUnFollow);
            await _repository.UpdateSocialUserAsync(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}