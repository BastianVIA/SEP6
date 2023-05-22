using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class RemovedRatingIntegrationEventSender : INotificationHandler<RemovedRatingEvent>
{
    private readonly IMediator _mediator;

    public RemovedRatingIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(RemovedRatingEvent notification, CancellationToken cancellationToken)
    {
        var newIntegrationEvent = new RemovedRatingIntegrationEvent(notification.UserId, notification.Rating.MovieId,
            notification.Rating.NumberOfStars);
        await _mediator.Publish(newIntegrationEvent, cancellationToken);
    }
}