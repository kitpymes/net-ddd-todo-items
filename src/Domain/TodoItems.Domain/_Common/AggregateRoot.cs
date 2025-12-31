using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoItems.Domain._Common;

public abstract class AggregateRoot : EntityBaseGuid, IAggregateRoot
{
    private readonly List<INotification> _domainEvents = [];

    [NotMapped]
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(INotification domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
