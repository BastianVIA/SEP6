using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class FavoritedMovieIntegrationEventSender : INotificationHandler<FavoritedMovie>
{
    private readonly IMediator _mediator;

    public FavoritedMovieIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(FavoritedMovie notification, CancellationToken cancellationToken)
    {
        var newIntegrationEvent = new FavoritedMovieIntegrationEvent(notification.UserId, notification.MovieId);
        await _mediator.Publish(newIntegrationEvent, cancellationToken);
    }
}