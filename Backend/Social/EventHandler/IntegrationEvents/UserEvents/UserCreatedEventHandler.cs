using Backend.Database.TransactionManager;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;
using NLog;

namespace Backend.Social.EventHandler.IntegrationEvents.UserEvents;

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
            var newUser = new SocialUser(notification.UserId);
            _socialUserRepository.CreateSocialUserAsync(newUser, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}