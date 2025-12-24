using MediatR;

namespace TodoItems.Domain.Events;

public class ItemCreatedEvent(int itemId) : DomainEvent, INotification
{
    public int ItemId { get; } = itemId;
}
