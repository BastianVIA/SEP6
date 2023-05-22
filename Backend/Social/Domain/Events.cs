using MediatR;

namespace Backend.SocialFeed.Domain;

public class SocialUserCreatedEvent : INotification
{
    public string Id { get; }
    
    public SocialUserCreatedEvent(string id)
    {
        Id = id;
    }
}