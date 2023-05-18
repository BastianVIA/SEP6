using Backend.Database.TransactionManager;
using Backend.SocialFeed.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;
using NLog;

namespace Backend.SocialFeed.EventHandler.IntegrationEvents.UserEvents;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedIntegrationEvent>
{
    private readonly ISocialUserRepository _socialUserRepository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public UserCreatedEventHandler(ISocialUserRepository socialUserRepository, IDatabaseTransactionFactory transactionFactory)
    {
        _socialUserRepository = socialUserRepository;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        LogManager.GetCurrentClassLogger().Info("Handling UserCreatedIntegrationEvent");
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            _socialUserRepository.CreateSocialUser(notification.UserId, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}