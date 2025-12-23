namespace TodoItems.Domain.Events;

public sealed class ItemCreatedEvent(int itemId) : DomainEvent
{
    public int ItemId { get; } = itemId;
}
