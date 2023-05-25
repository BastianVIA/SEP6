using Backend.Database.TransactionManager;
using Backend.Social.Domain;
using Backend.Social.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.Social.EventHandler.IntegrationEvents.UserEvents;

public class FavoritedMovieEventHandler : INotificationHandler<FavoritedMovieIntegrationEvent>
{
    private readonly IDatabaseTransactionFactory _transactionFactory;
    private readonly IPostRepository _repository;

    public FavoritedMovieEventHandler(IDatabaseTransactionFactory transactionFactory, IPostRepository repository)
    {
        _transactionFactory = transactionFactory;
        _repository = repository;
    }

    public async Task Handle(FavoritedMovieIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            var activityData = new ActivityData
            {
                MovieId = notification.MovieId
            };
            var newPost = new Post(notification.UserId, Activity.FavoriteMovie, activityData);
            await _repository.CreatePostAsync(newPost, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}