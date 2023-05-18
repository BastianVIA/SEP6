using Backend.User.Domain;
using Backend.User.IntegrationEvents;
using MediatR;

namespace Backend.User.EventHandler.IntegrationEventSender;

public class UserCreatedIntegrationEventSender : INotificationHandler<UserCreated>
{
    private readonly IMediator _mediator;

    public UserCreatedIntegrationEventSender(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        var newEvent = new UserCreatedIntegrationEvent(notification.UserId);
        await _mediator.Publish(newEvent, cancellationToken);
    }
}