using Backend.Database.TransactionManager;
using Backend.Movie.Infrastructure;
using Backend.User.IntegrationEvents;
using MediatR;
using NLog;

namespace Backend.Movie.EventHandler.IntegrationEvents.UserEvents;

public class CreatedRatingEventHandler : INotificationHandler<CreatedRatingIntegrationEvent>
{
    private readonly IMovieRepository _movieRepository;
    private readonly IDatabaseTransactionFactory _transactionFactory;

    public CreatedRatingEventHandler(IMovieRepository movieRepository, IDatabaseTransactionFactory transactionFactory)
    {
        _movieRepository = movieRepository;
        _transactionFactory = transactionFactory;
    }

    public async Task Handle(CreatedRatingIntegrationEvent notification, CancellationToken cancellationToken)
    {
        LogManager.GetCurrentClassLogger().Info("Handling CreatedRatingIntegrationEvent");
        var transaction = _transactionFactory.GetCurrentTransaction();
        try
        {
            var movie = await _movieRepository.ReadMovieFromId(notification.MovieId, transaction);
            movie.NewRating(notification.Rating);
            await _movieRepository.Update(movie, transaction);
        }
        catch (Exception e)
        {
            await transaction.RollbackTransactionAsync();
            throw;
        }

    }
}