using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class CreatedRatingIntegrationEventSender : INotificationHandler<CreatedRatingEvent>
{
    private readonly IMediator _mediator;

    public CreatedRatingIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(CreatedRatingEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("CreatedRatingIntegrationEvent Sender");
        var integrationEvent = new CreatedRatingIntegrationEvent(notification.UserId, notification.Rating.MovieId,
            notification.Rating.NumberOfStars);
        await _mediator.Publish(integrationEvent, cancellationToken);
    }
}