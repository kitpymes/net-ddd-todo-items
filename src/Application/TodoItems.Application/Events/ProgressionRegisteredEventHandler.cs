using TodoItems.Domain.Events;

namespace TodoItems.Application.Events;

public class ProgressionRegisteredEventHandler
{
    public Task Handle(ProgressionRegisteredEvent @event)
    {
        Console.WriteLine(
            $"[EVENT] Progreso registrado: {@event.ItemId} -> {@event.Percent}%"
        );

        return Task.CompletedTask;
    }
}