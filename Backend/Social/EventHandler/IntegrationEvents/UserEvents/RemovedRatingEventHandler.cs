using Backend.Database.TransactionManager;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.Social.EventHandler.IntegrationEvents.UserEvents;

public class RemovedRatingEventHandler : INotificationHandler<RemovedRatingIntegrationEvent>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _postRepository;

    public RemovedRatingEventHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository postRepository)
    {
        _transactionFactory = transactionFactory;
        _postRepository = postRepository;
    }

    public async Task Handle(RemovedRatingIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            var newActivity = new ActivityData
            {
                MovieId = notification.MovieId,
                OldRating = notification.PreviousRating
            };
            var newPost = new Post(notification.UserId, Activity.RemovedRating, newActivity);
            await _postRepository.CreatePostAsync(newPost, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }    }
}