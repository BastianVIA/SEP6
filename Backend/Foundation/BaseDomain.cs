using MediatR;

namespace Backend.Foundation;

public class BaseDomain
{
    public List<INotification> domainEvents { get; }

    public BaseDomain()
    {
        domainEvents = new List<INotification>();
    }

    public void AddDomainEvent(INotification domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public List<INotification> ReadAllDomainEvents()
    {
        var copiedEvents = domainEvents.ToList();
        domainEvents.Clear();
        return copiedEvents;
    }
}