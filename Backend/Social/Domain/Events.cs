using MediatR;

namespace Backend.Social.Domain;

public class SocialUserCreatedEvent : INotification
{
    public string Id { get; }
    
    public SocialUserCreatedEvent(string id)
    {
        Id = id;
    }
}

public class PostCreatedEvent : INotification
{
    public Guid Id { get; }

    public PostCreatedEvent(Guid id)
    {
        Id = id;
    }
}