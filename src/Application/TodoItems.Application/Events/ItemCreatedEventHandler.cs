using MediatR;
using Microsoft.Extensions.Logging;
using TodoItems.Domain.Events;

namespace TodoItems.Application.Events;

public class ItemCreatedEventHandler : INotificationHandler<ItemCreatedEvent>
{
    public Task Handle(ItemCreatedEvent notification, CancellationToken ct)
    {
        Console.WriteLine($"[EVENT] Item creado: {notification.ItemId}");
        return Task.CompletedTask;
    }
}
