using System.ComponentModel.DataAnnotations;
using Backend.Database.TransactionManager;
using Backend.SocialFeed.Infrastructure;
using MediatR;

namespace Backend.SocialFeed.Application.FollowUser;


public record Command(string userId, string userIdToFollow) : IRequest;

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
            var user = await _repository.ReadSocialUserAsync(request.userId, transaction, includeFollowing:true);
            if (user.AlreadyFollows(request.userIdToFollow))
            {
                throw new ValidationException($"User already follows {request.userIdToFollow}");
            }
            user.StartFollowing(request.userIdToFollow);
            await _repository.UpdateSocialUser(user, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }

    }
}