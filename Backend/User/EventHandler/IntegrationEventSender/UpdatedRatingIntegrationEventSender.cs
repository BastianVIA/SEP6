using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class UpdatedRatingIntegrationEventSender : INotificationHandler<UpdatedRatingEvent>
{
    private readonly IMediator _mediator;

    public UpdatedRatingIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdatedRatingEvent notification, CancellationToken cancellationToken)
    {
        var newIntegrationEvent = new UpdatedRatingIntegrationEvent(notification.UserId, notification.MovieId,
            notification.PreviousRating, notification.NewRating);
        await _mediator.Publish(newIntegrationEvent, cancellationToken);
    }
}