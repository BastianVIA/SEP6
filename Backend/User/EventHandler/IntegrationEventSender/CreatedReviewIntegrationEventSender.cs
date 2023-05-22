using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class CreatedReviewIntegrationEventSender : INotificationHandler<CreateReviewEvent>
{
    private readonly IMediator _mediator;

    public CreatedReviewIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Handle(CreateReviewEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new CreatedReviewIntegrationEvent(notification.UserId, notification.Review.MovieId,
            notification.Review.ReviewBody);
        await _mediator.Publish(integrationEvent);
    }
}