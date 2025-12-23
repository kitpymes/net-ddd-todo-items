using TodoItems.Domain.Events;

namespace TodoItems.Application.Events;

public class ItemCreatedEventHandler
{
    public Task Handle(ItemCreatedEvent @event)
    {
        Console.WriteLine($"[EVENT] Item creado: {@event.ItemId}");
        return Task.CompletedTask;
    }
}
