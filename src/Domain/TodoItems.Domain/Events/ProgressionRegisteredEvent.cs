namespace TodoItems.Domain.Events;

public sealed class ProgressionRegisteredEvent(int itemId, decimal percent) : DomainEvent
{
    public int ItemId { get; } = itemId;
    public decimal Percent { get; } = percent;
}