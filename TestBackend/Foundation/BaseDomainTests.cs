using Backend.Foundation;
using MediatR;

namespace TestBackend.Foundation;

using System.Linq;
using Xunit;

public class BaseDomainTests
{
    private BaseDomain baseDomain;

    public BaseDomainTests()
    {
        baseDomain = new BaseDomain();
    }

    [Fact]
    public void AddDomainEvent_AddEvent_ReturnsListOfDomainEvents()
    {
        var domainEvent = new TestDomainEvent();
        baseDomain.AddDomainEvent(domainEvent);

        var domainEvents = baseDomain.ReadAllDomainEvents();

        Assert.Single(domainEvents);
        Assert.Equal(domainEvent, domainEvents.First());
    }

    [Fact]
    public void ReadAllDomainEvents_NoEvents_ReturnsEmptyList()
    {
        var domainEvents = baseDomain.ReadAllDomainEvents();

        Assert.Empty(domainEvents);
    }

    [Fact]
    public void ReadAllDomainEvens_WithEvents_ClearsTheList()
    {
        var domainEvent = new TestDomainEvent();
        baseDomain.AddDomainEvent(domainEvent);

        baseDomain.ReadAllDomainEvents();
        var domainEvents = baseDomain.ReadAllDomainEvents();

        Assert.Empty(domainEvents);
    }

    private class TestDomainEvent : INotification
    {
    }
}