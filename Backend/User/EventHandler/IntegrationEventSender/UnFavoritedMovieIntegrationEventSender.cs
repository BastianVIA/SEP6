using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class UnFavoritedMovieIntegrationEventSender : INotificationHandler<UnFavoritedMovie>
{
    private readonly IMediator _mediator;

    public UnFavoritedMovieIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UnFavoritedMovie notification, CancellationToken cancellationToken)
    {
        var newIntegrationEvent = new UnFavoritedMovieIntegrationEvent(notification.UserId, notification.MovieId);
        await _mediator.Publish(newIntegrationEvent, cancellationToken);
    }
}