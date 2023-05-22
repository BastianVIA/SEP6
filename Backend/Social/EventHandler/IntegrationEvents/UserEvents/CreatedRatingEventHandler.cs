using Backend.Database.TransactionManager;
using Backend.SocialFeed.Domain;
using Backend.SocialFeed.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.SocialFeed.EventHandler.IntegrationEvents.UserEvents;

public class CreatedRatingEventHandler : INotificationHandler<CreatedRatingIntegrationEvent>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _postRepository;

    public CreatedRatingEventHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository postRepository)
    {
        _transactionFactory = transactionFactory;
        _postRepository = postRepository;
    }

    public async Task Handle(CreatedRatingIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            var newActivity = new ActivityData
            {
                MovieId = notification.MovieId,
                NewRating = notification.Rating
            };
            var newPost = new Post(notification.UserId, Activity.CreatedRating, newActivity);
            await _postRepository.CreatePostAsync(newPost, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}