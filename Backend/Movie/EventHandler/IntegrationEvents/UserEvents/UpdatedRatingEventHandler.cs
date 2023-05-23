using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;
using NLog;

namespace Backend.Movie.EventHandler.IntegrationEvents.UserEvents;

public class UpdatedRatingEventHandler : INotificationHandler<UpdatedRatingIntegrationEvent>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public UpdatedRatingEventHandler(IMovieRepository movieRepository, IDatabaseTransactionFactory transactionFactory)
    {
        _movieRepository = movieRepository;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(UpdatedRatingIntegrationEvent notification, CancellationToken cancellationToken)
    {
        LogManager.GetCurrentClassLogger().Info("Handling UpdatedRatingIntegrationEvent");
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            var movie = await _movieRepository.ReadMovieFromIdAsync(notification.MovieId, transaction, includeRatings:true);
            movie.UpdateRating(notification.PreviousRating, notification.NewRating);
            await _movieRepository.Update(movie, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }
    }
}